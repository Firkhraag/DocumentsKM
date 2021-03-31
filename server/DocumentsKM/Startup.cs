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
using DocumentsKM.Services;
using DocumentsKM.Helpers;
using System.Data;
using System.Data.SqlClient;

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
                    builder.WithOrigins("http://localhost:5000", "http://localhost:5001")
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
            // Local URI: https://localhost:5001/swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "DocumentsKM",
                        Version = "v1",
                        Description = "Сервис марок КМ"
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
                    ClockSkew = TimeSpan.Zero
                };
            });

              // Подключение к базе данных ArchDocT
            services.AddTransient<IDbConnection>((s) => new SqlConnection(
                this.Configuration.GetConnectionString("ArchiveConnectionString")));

            // Подключение к базе данных DocumentsKM
            services.AddDbContext<ApplicationContext>(
                opt => opt.UseLazyLoadingProxies()
                    .UseSqlServer(
                        Configuration.GetConnectionString("SQLServerConnection")
                    ));

            services.AddAutoMapper(typeof(Startup));

            // Http client for api requests
            services.AddHttpClient();

            services.AddHostedService<FetchService>();

            // SPA
            services.AddSpaStaticFiles(configuration => 
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // DI for application services
            InjectScopedServices(services);
            InjectScopedRepositories(services);
        }

        private void InjectScopedServices(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<ISubnodeService, SubnodeService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IMarkService, MarkService>();
            services.AddScoped<IMarkApprovalService, MarkApprovalService>();

            services.AddScoped<IConstructionTypeService, ConstructionTypeService>();
            services.AddScoped<IConstructionSubtypeService, ConstructionSubtypeService>();
            services.AddScoped<IWeldingControlService, WeldingControlService>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<IStandardConstructionService, StandardConstructionService>();
            services.AddScoped<IStandardConstructionNameService, StandardConstructionNameService>();
            services.AddScoped<IConstructionService, ConstructionService>();
            services.AddScoped<IBoltDiameterService, BoltDiameterService>();
            services.AddScoped<IConstructionBoltService, ConstructionBoltService>();

            services.AddScoped<IProfileClassService, ProfileClassService>();
            services.AddScoped<ISteelService, SteelService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IConstructionElementService, ConstructionElementService>();

            services.AddScoped<IDocService, DocService>();
            services.AddScoped<ISheetNameService, SheetNameService>();
            services.AddScoped<IDocTypeService, DocTypeService>();

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
            services.AddScoped<IAdditionalWorkService, AdditionalWorkService>();

            services.AddScoped<IEstimateTaskService, EstimateTaskService>();

            services.AddScoped<IGeneralDataSectionService, GeneralDataSectionService>();
            services.AddScoped<IGeneralDataPointService, GeneralDataPointService>();
            services.AddScoped<IMarkGeneralDataSectionService, MarkGeneralDataSectionService>();
            services.AddScoped<IMarkGeneralDataPointService, MarkGeneralDataPointService>();

            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ISpecificationDocumentService, SpecificationDocumentService>();
            services.AddScoped<IGeneralDataDocumentService, GeneralDataDocumentService>();
            services.AddScoped<IConstructionDocumentService, ConstructionDocumentService>();
            services.AddScoped<IBoltDocumentService, BoltDocumentService>();
            services.AddScoped<IEstimateTaskDocumentService, EstimateTaskDocumentService>();
            services.AddScoped<IProjectRegistrationDocumentService, ProjectRegistrationDocumentService>();
            services.AddScoped<IEstimationTitleDocumentService, EstimationTitleDocumentService>();
            services.AddScoped<IEstimationPagesDocumentService, EstimationPagesDocumentService>();

            services.AddScoped<ICorrProtGeneralDataPointService, CorrProtGeneralDataPointService>();
            services.AddScoped<IDefaultValuesService, DefaultValuesService>();
        }

        private void InjectScopedRepositories(IServiceCollection services)
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

            services.AddScoped<IConstructionTypeRepo, SqlConstructionTypeRepo>();
            services.AddScoped<IConstructionSubtypeRepo, SqlConstructionSubtypeRepo>();
            services.AddScoped<IWeldingControlRepo, SqlWeldingControlRepo>();
            services.AddScoped<ISpecificationRepo, SqlSpecificationRepo>();
            services.AddScoped<IStandardConstructionRepo, SqlStandardConstructionRepo>();
            services.AddScoped<IStandardConstructionNameRepo, SqlStandardConstructionNameRepo>();
            services.AddScoped<IConstructionRepo, SqlConstructionRepo>();
            services.AddScoped<IBoltDiameterRepo, SqlBoltDiameterRepo>();
            services.AddScoped<IBoltLengthRepo, SqlBoltLengthRepo>();
            services.AddScoped<IConstructionBoltRepo, SqlConstructionBoltRepo>();

            services.AddScoped<IProfileTypeRepo, SqlProfileTypeRepo>();
            services.AddScoped<IProfileClassRepo, SqlProfileClassRepo>();
            services.AddScoped<ISteelRepo, SqlSteelRepo>();
            services.AddScoped<IProfileRepo, SqlProfileRepo>();
            services.AddScoped<IConstructionElementRepo, SqlConstructionElementRepo>();

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
            services.AddScoped<IAdditionalWorkRepo, SqlAdditionalWorkRepo>();

            services.AddScoped<IEstimateTaskRepo, SqlEstimateTaskRepo>();

            services.AddScoped<IGeneralDataSectionRepo, SqlGeneralDataSectionRepo>();
            services.AddScoped<IGeneralDataPointRepo, SqlGeneralDataPointRepo>();
            services.AddScoped<IMarkGeneralDataSectionRepo, SqlMarkGeneralDataSectionRepo>();
            services.AddScoped<IMarkGeneralDataPointRepo, SqlMarkGeneralDataPointRepo>();

            services.AddScoped<ICorrProtVariantRepo, SqlCorrProtVariantRepo>();
            services.AddScoped<ICorrProtMethodRepo, SqlCorrProtMethodRepo>();
            services.AddScoped<ICorrProtCoatingRepo, SqlCorrProtCoatingRepo>();
            services.AddScoped<ICorrProtCleaningDegreeRepo, SqlCorrProtCleaningDegreeRepo>();
            services.AddScoped<IPrimerRepo, SqlPrimerRepo>();

            services.AddScoped<IDefaultValuesRepo, SqlDefaultValuesRepo>();
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

            // Logger
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
