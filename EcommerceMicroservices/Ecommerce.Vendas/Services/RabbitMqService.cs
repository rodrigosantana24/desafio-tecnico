using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

public class RabbitMqService
{
    public void SendMessage(object message)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "order-created-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "order-created-queue", basicProperties: null, body: body);
        }
    }
}