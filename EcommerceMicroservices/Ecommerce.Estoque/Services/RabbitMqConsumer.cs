using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private RabbitMQ.Client.IConnection _connection;
    private RabbitMQ.Client.IModel _channel;

    public RabbitMqConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // connection will be created in ExecuteAsync with retries so app doesn't crash
    }

    private ConnectionFactory CreateFactory()
    {
        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
        return new RabbitMQ.Client.ConnectionFactory() { HostName = host };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = CreateFactory();

        // Retry until RabbitMQ is available or cancellation requested
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "order-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMqConsumer] RabbitMQ not ready: {ex.Message}. Retrying in 2s...");
                try { await Task.Delay(2000, stoppingToken); } catch { break; }
            }
        }

        if (_channel == null)
        {
            Console.WriteLine("[RabbitMqConsumer] Could not establish RabbitMQ channel, exiting consumer.");
            return;
        }

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var orderEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

            if (orderEvent != null)
            {
                await AtualizarEstoque(orderEvent);
            }
        };

        _channel.BasicConsume(queue: "order-created-queue", autoAck: true, consumer: consumer);

        // keep running until stopped
        await Task.Delay(-1, stoppingToken).ContinueWith(_ => { });
    }

    private async Task AtualizarEstoque(OrderCreatedEvent orderEvent)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EstoqueContext>();
            var produto = await context.Produtos.FindAsync(orderEvent.ProductId);
            if (produto != null)
            {
                produto.Estoque -= orderEvent.Quantity;
                if (produto.Estoque < 0) produto.Estoque = 0;
                await context.SaveChangesAsync();
                Console.WriteLine($"[Estoque] Produto {produto.Nome} atualizado. Novo estoque: {produto.Estoque}");
            }
        }
    }
}