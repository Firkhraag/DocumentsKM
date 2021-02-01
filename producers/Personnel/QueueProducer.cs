using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Personnel
{
    public class QueueProducer
    {
        public static void Publish(IModel channel)
        {
            channel.QueueDeclare(
                "personnel-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var count = 0;

            while (count < 10)
            {
                var message = new { Name = "Test", Message = $"Hello {count}" };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish("", "personnel-queue", null, body);
                count++;
            }
        }
    }
}
