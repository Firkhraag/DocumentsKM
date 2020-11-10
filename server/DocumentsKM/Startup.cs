using System;
using AutoMapper;
using DocumentsKM.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    builder.WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // Controllers
            // Игнорировать JsonIgnore свойства
            services.AddControllers()
                .AddJsonOptions(
                    opt =>
                    {
                        opt.JsonSerializerOptions.IgnoreNullValues = true;
                        opt.JsonSerializerOptions.Converters.Add(new TrimConverter());
                    }
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

            // Подключение к базе данных
            // Postgres
            services.AddDbContext<ApplicationContext>(
                opt => opt.UseLazyLoadingProxies()
                    .UseNpgsql(
                        Configuration.GetConnectionString("PostgresConnection")
                    ));

            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetConnectionString("ReddisConnection")));
            services.AddSingleton<ICacheService, RedisCacheService>();

            // DI for application services
            injectScopedServices(services);
            injectScopedRepositories(services);
        }

        private void injectScopedServices(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<ISubnodeService, SubnodeService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IMarkService, MarkService>();
            services.AddScoped<IMarkApprovalService, MarkApprovalService>();
            services.AddScoped<ISpecificationService, SpecificationService>();

            services.AddScoped<IDocService, DocService>();
            services.AddScoped<ISheetNameService, SheetNameService>();

            services.AddScoped<IMarkLinkedDocService, MarkLinkedDocService>();
            services.AddScoped<ILinkedDocService, LinkedDocService>();
            services.AddScoped<ILinkedDocTypeService, LinkedDocTypeService>();

            services.AddScoped<IEnvAggressivenessService, EnvAggressivenessService>();
            services.AddScoped<IOperatingAreaService, OperatingAreaService>();
            services.AddScoped<IGasGroupService, GasGroupService>();
            services.AddScoped<IConstructionMaterialService, ConstructionMaterialService>();
            services.AddScoped<IPaintworkTypeService, PaintworkTypeService>();
            services.AddScoped<IHighTensileBoltsTypeService, HighTensileBoltsTypeService>();
            services.AddScoped<IMarkOperatingConditionsService, MarkOperatingConditionsService>();

            services.AddScoped<IAttachedDocService, AttachedDocService>();
        }

        private void injectScopedRepositories(IServiceCollection services)
        {
            services.AddScoped<IProjectRepo, SqlProjectRepo>();
            services.AddScoped<INodeRepo, SqlNodeRepo>();
            services.AddScoped<ISubnodeRepo, SqlSubnodeRepo>();
            services.AddScoped<IEmployeeRepo, SqlEmployeeRepo>();
            services.AddScoped<IDepartmentRepo, SqlDepartmentRepo>();
            services.AddScoped<IPositionRepo, SqlPositionRepo>();
            services.AddScoped<IUserRepo, SqlUserRepo>();

            services.AddScoped<IMarkRepo, SqlMarkRepo>();
            services.AddScoped<IMarkApprovalRepo, SqlMarkApprovalRepo>();
            services.AddScoped<ISpecificationRepo, SqlSpecificationRepo>();

            services.AddScoped<IDocRepo, SqlDocRepo>();
            services.AddScoped<ISheetNameRepo, SqlSheetNameRepo>();
            services.AddScoped<IDocTypeRepo, SqlDocTypeRepo>();

            services.AddScoped<IMarkLinkedDocRepo, SqlMarkLinkedDocRepo>();
            services.AddScoped<ILinkedDocRepo, SqlLinkedDocRepo>();
            services.AddScoped<ILinkedDocTypeRepo, SqlLinkedDocTypeRepo>();

            services.AddScoped<IEnvAggressivenessRepo, SqlEnvAggressivenessRepo>();
            services.AddScoped<IOperatingAreaRepo, SqlOperatingAreaRepo>();
            services.AddScoped<IGasGroupRepo, SqlGasGroupRepo>();
            services.AddScoped<IConstructionMaterialRepo, SqlConstructionMaterialRepo>();
            services.AddScoped<IPaintworkTypeRepo, SqlPaintworkTypeRepo>();
            services.AddScoped<IHighTensileBoltsTypeRepo, SqlHighTensileBoltsTypeRepo>();
            services.AddScoped<IMarkOperatingConditionsRepo, SqlMarkOperatingConditionsRepo>();

            services.AddScoped<IAttachedDocRepo, SqlAttachedDocRepo>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");

            app.UseCors("EnableCORS");

            // Swagger UI
            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentsKM API");
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
