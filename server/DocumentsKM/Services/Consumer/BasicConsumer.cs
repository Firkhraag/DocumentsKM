using DocumentsKM.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;
using System.Text.Json;

namespace DocumentsKM.Services
{
    public class BasicConsumer
    {
        protected IModel _model;
        private bool _disposed;
        protected readonly string _exchange;
        protected string _queue;
        protected string _key;

        public BasicConsumer(
            IConnectionProvider connectionProvider,
            string exchange,
            string queue,
            string routingKey)
        {
            _model = connectionProvider.GetConnection().CreateModel();
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
            _exchange = exchange;
            _queue = queue;
            _key = routingKey;
            Log.Information("Connected to RabbitMQ");
        }

        protected virtual void OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            Log.Information("AMQP Message was received");
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var h = @event.BasicProperties.Headers;
                var message = JsonSerializer.Deserialize<T>(body);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error while retrieving message from queue.");
            }
            finally
            {
                _model.BasicAck(@event.DeliveryTag, false);
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
