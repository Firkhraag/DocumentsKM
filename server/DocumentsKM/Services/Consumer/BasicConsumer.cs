using DocumentsKM.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DocumentsKM.Services
{
    public class BasicConsumer
    {
        protected IModel _model;
        private IConnection _connection;
        protected readonly string _exchange;
        protected string _queue;
        protected string _key;

        public BasicConsumer(
            ConnectionFactory connectionFactory,
            string exchange,
            string queue,
            string routingKey)
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = connectionFactory.CreateConnection();
            }
            if (_model == null || _model.IsOpen == false)
            {
                _model = _connection.CreateModel();
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
        }

        protected virtual void OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var headers = @event.BasicProperties.Headers;
                Log.Information(
                    "AMQP Message was received: {" + string.Join(
                        ",", headers.Select(
                            kv => kv.Key + "=" + System.Text.Encoding.UTF8.GetString(
                                (byte[])kv.Value)).ToArray()) + "}");
                if (headers.ContainsKey("entity") && headers.ContainsKey("method"))
                {
                    var entity = System.Text.Encoding.UTF8.GetString((byte[])headers["entity"]);
                    var method = System.Text.Encoding.UTF8.GetString((byte[])headers["method"]);
                    if (entity == "department" && method == "add")
                    {
                        Log.Information("AMQP Adding department");
                        // var department = JsonSerializer.Deserialize<Department>(message);

                        // using (var scope = _serviceScopeFactory.CreateScope())
                        // {
                        //     var departmentRepo = scope.ServiceProvider.GetRequiredService<IDepartmentRepo>();
                            
                        //     if (departmentRepo.GetById(department.Id) == null)
                        //     departmentRepo.Add(department);
                        //     else
                        //         Log.Information($"[AMQP] Department with id {department.Id} already exists");
                        // }
                    }
                }
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
            try
            {
                _model?.Close();
                _model?.Dispose();
                _model = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }

        // public void Dispose()
        // {
        //     Dispose(true);
        //     GC.SuppressFinalize(this);
        // }

        // protected virtual void Dispose(bool disposing)
        // {
        //     if (_disposed)
        //         return;

        //     if (disposing)
        //         _model?.Close();

        //     _disposed = true;
        // }
    }
}
