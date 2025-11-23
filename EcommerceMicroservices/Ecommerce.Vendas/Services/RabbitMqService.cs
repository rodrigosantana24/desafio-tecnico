using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

public class RabbitMqService
{
    public void SendMessage(object message)
    {
        try
        {
            // Use Docker service hostname when running in compose
            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "order-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "", routingKey: "order-created-queue", basicProperties: null, body: body);
            }
        }
        catch (Exception ex)
        {
            // Log and swallow to avoid failing the request; later you can add retry/backoff
            Console.WriteLine("[RabbitMqService] Failed to send message: " + ex.Message);
        }
    }
}