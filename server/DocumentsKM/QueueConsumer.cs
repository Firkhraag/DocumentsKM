using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DocumentsKM
{
    public class QueueConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare(
                "personnel-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume("personnel-queue", true, consumer);
        }
    }
}
