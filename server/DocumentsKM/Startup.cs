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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            // services.AddCors();
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

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(opt =>
            //     {
            //         // Resource id of api
            //         opt.Audience = Configuration["AAD:ResourceId"];
            //     });

            // services.AddAuthentication(opt =>
            // {
            //     opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            // .AddJwtBearer(opt =>
            // {

            // });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Development mode
                    ValidIssuer = "https://localhost:5001",
                    // ValidAudience = "https://localhost:5001",
                    ValidAudience = "http://localhost:8080",
                    // Should be stored in env
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            // Add Swagger documentation
            // URI: https://localhost:5001/swagger
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
            services.AddScoped<IEmployeeRepo, MockEmployeeRepo>();
            services.AddScoped<IDepartmentRepo, MockDepartmentRepo>();
            services.AddScoped<IPositionRepo, MockPositionRepo>();
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
            // app.UseCors(builder => builder.AllowAnyOrigin());
            // Prod
            // app.UseCors(builder => builder.WithOrigins("http://example.com"));
            app.UseCors("EnableCORS");

            app.UseStaticFiles();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

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
