using System;
using System.Linq;
using AutoMapper;
using DocumentsKM.Data;
using DocumentsKM.Profiles;
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

            services.AddAutoMapper(typeof(TStartup));

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
        context.Positions.AddRange(TestData.positions);
        context.Departments.AddRange(TestData.departments);
        context.Employees.AddRange(TestData.employees);

        context.ConstructionMaterials.AddRange(TestData.constructionMaterials);
        context.EnvAggressiveness.AddRange(TestData.envAggressiveness);
        context.GasGroups.AddRange(TestData.gasGroups);
        context.HighTensileBoltsTypes.AddRange(TestData.highTensileBoltsTypes);
        context.PaintworkTypes.AddRange(TestData.paintworkTypes);
        context.OperatingAreas.AddRange(TestData.operatingAreas);
        context.WeldingControl.AddRange(TestData.weldingControl);
        context.ConstructionTypes.AddRange(TestData.constructionTypes);
        context.ConstructionSubtypes.AddRange(TestData.constructionSubtypes);
        context.GeneralDataSections.AddRange(TestData.generalDataSections);
        context.SheetNames.AddRange(TestData.sheetNames);
        context.DocTypes.AddRange(TestData.docTypes);
        context.LinkedDocTypes.AddRange(TestData.linkedDocTypes);

        context.Projects.AddRange(TestData.projects);
        context.Nodes.AddRange(TestData.nodes);
        context.Subnodes.AddRange(TestData.subnodes);
        context.Marks.AddRange(TestData.marks);

        context.MarkApprovals.AddRange(TestData.markApprovals);
        context.Specifications.AddRange(TestData.specifications);
        
        context.AttachedDocs.AddRange(TestData.attachedDocs);
        context.LinkedDocs.AddRange(TestData.linkedDocs);
        context.MarkLinkedDocs.AddRange(TestData.markLinkedDocs);
        context.SaveChanges();
    }
}