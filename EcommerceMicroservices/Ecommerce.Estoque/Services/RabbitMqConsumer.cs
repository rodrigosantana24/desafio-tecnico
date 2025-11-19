using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System;

public class OrderCreatedEvent { public int ProductId { get; set; } public int Quantity { get; set; } }

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "order-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var orderEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);
            await AtualizarEstoque(orderEvent);
        };
        _channel.BasicConsume(queue: "order-created-queue", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task AtualizarEstoque(OrderCreatedEvent orderEvent)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            Console.WriteLine($"[Estoque] Baixa de {orderEvent.Quantity} itens no Produto ID {orderEvent.ProductId}");
            await Task.CompletedTask; 
        }
    }
}
