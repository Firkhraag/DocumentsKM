using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Personnel.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Personnel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var factory = new ConnectionFactory()
            // {
            //     Uri = new Uri("amqp://guest:guest@localhost:5672"),
            // };
            // using var connection = factory.CreateConnection();
            // using var channel = connection.CreateModel();
            // QueueProducer.Publish(channel);

            // Создание конфигурации, используя appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationContext>();
                }
                catch (Exception)
                {
                    return;
                }
            }

            try
            {
                host.Run();
            }
            catch (Exception)
            {
                return;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
