using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using DocumentsKM.Helpers;

namespace DocumentsKM.Services
{
    public class ConsumerService : BasicConsumer, IHostedService
    {
        public ConsumerService(
            IConnectionProvider connectionProvider,
            string exchange,
            string queue,
            string routingKey) :
            base(connectionProvider, exchange, queue, routingKey)
        {
            try
            {
                var consumer = new EventingBasicConsumer(_model);
                consumer.Received += OnEventReceived<Department>;
                _model.BasicConsume(_queue, false, consumer);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
