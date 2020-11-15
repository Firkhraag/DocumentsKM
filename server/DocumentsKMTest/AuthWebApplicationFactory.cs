using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class AuthWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationContext>));

            services.Remove(descriptor);

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<AuthWebApplicationFactory<TStartup>>>();

                context.Database.EnsureCreated();

                try
                {
                    InitializeDbForTests(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex, "An error occurred while seeding the database. Error: {Message}", ex.Message);
                }
            }
        });
    }

    private void InitializeDbForTests(ApplicationContext context)
    {
        context.ConstructionMaterials.AddRange(TestData.constructionMaterials);
        context.SaveChanges();
        context.ConstructionMaterials.AttachRange(TestData.constructionMaterials);
        // if (!context.ConstructionMaterials.Any())
        // {
        //     context.ConstructionMaterials.AddRange(TestData.constructionMaterials);
        //     context.SaveChanges();
        //     context.ConstructionMaterials.AttachRange(TestData.constructionMaterials);
        // }
    }
}