using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class TestWebApplicationFactory<TStartup>
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
                    .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

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
        context.ConstructionMaterials.AttachRange(TestData.constructionMaterials);

        context.EnvAggressiveness.AddRange(TestData.envAggressiveness);
        context.EnvAggressiveness.AttachRange(TestData.envAggressiveness);

        context.GasGroups.AddRange(TestData.gasGroups);
        context.GasGroups.AttachRange(TestData.gasGroups);

        context.HighTensileBoltsTypes.AddRange(TestData.highTensileBoltsTypes);
        context.HighTensileBoltsTypes.AttachRange(TestData.highTensileBoltsTypes);

        context.PaintworkTypes.AddRange(TestData.paintworkTypes);
        context.PaintworkTypes.AttachRange(TestData.paintworkTypes);

        context.WeldingControl.AddRange(TestData.weldingControl);
        context.WeldingControl.AttachRange(TestData.weldingControl);

        context.Projects.AddRange(TestData.projects);
        context.Projects.AttachRange(TestData.projects);

        context.Nodes.AddRange(TestData.nodes);
        context.Nodes.AttachRange(TestData.nodes);

        context.Subnodes.AddRange(TestData.subnodes);
        context.Subnodes.AttachRange(TestData.subnodes);

        context.Marks.AddRange(TestData.marks);
        context.Marks.AttachRange(TestData.marks);

        context.SaveChanges();
    }
}