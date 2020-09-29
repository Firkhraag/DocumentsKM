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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StackExchange.Redis;
using DocumentsKM.Services;
using DocumentsKM.Helpers;

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
            // CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("EnableCORS", builder =>
                {
                    // builder.WithOrigins("https://localhost:8080")
                    // .AllowAnyHeader()
                    // .AllowAnyMethod();
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Controllers
            // Игнорировать JsonIgnore свойства
            services.AddControllers()
                .AddJsonOptions(
                    opt => opt.JsonSerializerOptions.IgnoreNullValues = true
                );

            // Add Swagger documentation
            // URI: https://localhost:5001/swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "DocumentsKM",
                        Version = "v1",
                        Description = "Service is used for reading, creating and updating marks"
                    }
                );
            });

            // App settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JWTSecret);
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Токен будет истекать точно в ExpirationTime
                    ClockSkew = TimeSpan.Zero
                };
            });

            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetConnectionString("ReddisConnection")));
            services.AddSingleton<ICacheService, RedisCacheService>();

            // DI for application services
            services.AddScoped<IUserService, UserService>();

            injectScopedServices(services);
            injectScopedRepositories(services);
        }

        public void injectScopedServices(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<ISubnodeService, SubnodeService>();
            services.AddScoped<IMarkService, MarkService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserService, UserService>();
        }

        public void injectScopedRepositories(IServiceCollection services)
        {
            services.AddScoped<IProjectRepo, MockProjectRepo>();
            services.AddScoped<INodeRepo, MockNodeRepo>();
            services.AddScoped<ISubnodeRepo, MockSubnodeRepo>();
            services.AddScoped<IMarkRepo, MockMarkRepo>();
            services.AddScoped<IEmployeeRepo, MockEmployeeRepo>();
            services.AddScoped<IDepartmentRepo, MockDepartmentRepo>();
            services.AddScoped<IPositionRepo, MockPositionRepo>();
            services.AddScoped<IUserRepo, MockUserRepo>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // add hardcoded test user to db on startup,  
            // plain text password is used for simplicity, hashed passwords should be used in production applications
            // context.Users.Add(new User { FirstName = "Test", LastName = "User", Username = "test", Password = "test" });
            // context.SaveChanges();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseCors("EnableCORS");

            // Swagger UI
            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            // Requests logger
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
