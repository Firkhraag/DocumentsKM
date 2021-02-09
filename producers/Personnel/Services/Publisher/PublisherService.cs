using Personnel.Helpers;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Personnel.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IModel _model;
        private bool _disposed;
        private readonly string _exchange;
        private string _key;

        public PublisherService(
            IConnectionProvider connectionProvider,
            string exchange,
            string queue,
            string routingKey)
        {
            _model = connectionProvider.GetConnection().CreateModel();
            _exchange = exchange;
            _model.ExchangeDeclare(
                exchange: exchange, 
                type: "direct", 
                durable: true, 
                autoDelete: false);
            _model.QueueDeclare(
                queue: queue, 
                durable: true,
                exclusive: false, 
                autoDelete: false);
            _model.QueueBind(
                queue: queue, 
                exchange: exchange, 
                routingKey: routingKey);
            _key = routingKey;
            Log.Information("Connected to RabbitMQ");
        }

        public void Publish(
            string message,
            IDictionary<string, object> messageAttributes,
            string timeToLive = "30000")
        {
             try
            {
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _model.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";
                properties.Headers = messageAttributes;
                properties.Expiration = timeToLive;
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                
                _model.BasicPublish(_exchange, _key, properties, body);
                Log.Information("AMQP Message was published");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error while publishing");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _model?.Close();

            _disposed = true;
        }
    }
}

// using Personnel.Helpers;
// using RabbitMQ.Client;
// using System;
// using System.Collections.Generic;
// using System.Text;

// namespace Personnel.Services
// {
//     public class PublisherService : IPublisherService
//     {
//         private readonly IConnectionProvider _connectionProvider;
//         private readonly string _exchange;
//         private readonly IModel _model;
//         private bool _disposed;

//         public PublisherService(
//             IConnectionProvider connectionProvider,
//             string exchange,
//             string exchangeType,
//             int timeToLive = 30000)
//         {
//             _connectionProvider = connectionProvider;
//             _exchange = exchange;
//             _model = _connectionProvider.GetConnection().CreateModel();
//             var ttl = new Dictionary<string, object>
//             {
//                 {"x-message-ttl", timeToLive }
//             };
//             _model.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
//         }

//         public void Publish(
//             string message,
//             string routingKey,
//             IDictionary<string, object> messageAttributes,
//             string timeToLive = "30000")
//         {
//             var body = Encoding.UTF8.GetBytes(message);
//             var properties = _model.CreateBasicProperties();
//             properties.Persistent = true;
//             properties.Headers = messageAttributes;
//             properties.Expiration = timeToLive;

//             _model.BasicPublish(_exchange, routingKey, properties, body);
//         }

//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }

//         // Protected implementation of Dispose pattern.
//         protected virtual void Dispose(bool disposing)
//         {
//             if (_disposed)
//                 return;

//             if (disposing)
//                 _model?.Close();

//             _disposed = true;
//         }
//     }
// }