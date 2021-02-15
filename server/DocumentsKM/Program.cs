using System;
using DocumentsKM.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DocumentsKM
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
            // QueueConsumer.Consume(channel);

            // Создание конфигурации, используя appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Создание Serilog логгера
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while seeding the database");
                    return;
                }
            }

            try
            {
                Log.Information("Application starting up");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                Log.Information("Application is shutting down");
                // Записываем оставшиеся логи
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                // DI Serilog логгера
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
