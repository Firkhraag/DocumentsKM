using System;
using AutoMapper;
using DocumentsKM.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Newtonsoft.Json.Serialization;

namespace DocumentsKM
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

            // Add CORS services
            services.AddCors();

            // Add Swagger documentation
            // URI: https://localhost:8081/swagger
            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Marks Data",
                            Version = "v1",
                            Description = "Service is used for reading, creating and updating marks"
                        }
                    );
                });

            // Configure connection to database
            // Postgres
            services.AddDbContext<MarkContext>(opt => opt.UseNpgsql(
                Configuration.GetConnectionString("MarksDataConnection")
            ));

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Inject MockRepos into IRepos
            services.AddScoped<IProjectRepo, MockProjectRepo>();
            services.AddScoped<INodeRepo, MockNodeRepo>();
            services.AddScoped<ISubnodeRepo, MockSubnodeRepo>();
            services.AddScoped<IMarkRepo, MockMarkRepo>();
            // Inject SqlRepos into IRepos
            // services.AddScoped<IMarkRepo, SqlMarkRepo>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable CORS
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // SECURITY BREACH, REPLACE TO SECOND LINE WHEN USING FRONTEND SERVER
            // Dev
            app.UseCors(builder => builder.AllowAnyOrigin());
            // Prod
            // app.UseCors(builder => builder.WithOrigins("http://example.com"));

            app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
