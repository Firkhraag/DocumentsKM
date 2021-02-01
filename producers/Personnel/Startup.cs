using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Personnel.Data;
using Personnel.Helpers;
using Personnel.Services;
using RabbitMQ.Client;

namespace Personnel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("EnableCORS", builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            services.AddControllers()
                .AddJsonOptions(
                    opt =>
                    {
                        opt.JsonSerializerOptions.IgnoreNullValues = true;
                        opt.JsonSerializerOptions.Converters.Add(new TrimConverter());
                    }
                );

            services.AddDbContext<ApplicationContext>(
                opt => opt.UseNpgsql(
                    Configuration.GetConnectionString("PostgresConnection")
                ));

            services.AddAutoMapper(typeof(Startup));

            // services.AddSingleton<IConnectionProvider>(
            //     new ConnectionProvider(Configuration.GetConnectionString("RabbitMQConnection")));

            // services.AddScoped<IPublisherService>(
            //     x => new PublisherService(x.GetService<IConnectionProvider>(),
            //         "personnel_exchange", ExchangeType.Topic));

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IDepartmentRepo, SqlDepartmentRepo>();
            services.AddScoped<IPositionRepo, SqlPositionRepo>();
            services.AddScoped<IEmployeeRepo, SqlEmployeeRepo>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
