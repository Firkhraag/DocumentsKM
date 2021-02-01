using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DocumentsKM
{
    public class DirectExchangePublisher
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare(
                "personnel-direct-exchange",
                ExchangeType.Direct);
            channel.QueueDeclare(
                "personnel-direct-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("personnel-direct-queue", "personnel-direct-exchange", "personnel.init");
            channel.BasicQos(0, 10, false);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume("personnel-direct-queue", true, consumer);
        }
    }
}
