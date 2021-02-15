using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkOperatingConditionsRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<MarkOperatingConditions> markOperatingConditions)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkOperatingConditionsTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ConstructionMaterials.AddRange(TestData.constructionMaterials);
            context.EnvAggressiveness.AddRange(TestData.envAggressiveness);
            context.GasGroups.AddRange(TestData.gasGroups);
            context.HighTensileBoltsTypes.AddRange(TestData.highTensileBoltsTypes);
            context.PaintworkTypes.AddRange(TestData.paintworkTypes);
            context.OperatingAreas.AddRange(TestData.operatingAreas);
            context.WeldingControl.AddRange(TestData.weldingControl);
            context.MarkOperatingConditions.AddRange(markOperatingConditions);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetByMarkId_ShouldReturnMarkOperatingConditions()
        {
            // Arrange
            var context = GetContext(TestData.markOperatingConditions);
            var repo = new SqlMarkOperatingConditionsRepo(context);

            var markId = _rnd.Next(1, TestData.marks.Count());

            // Act
            var markOperatingConditions = repo.GetByMarkId(markId);

            // Assert
            Assert.Equal(
                TestData.markOperatingConditions.FirstOrDefault(v => v.Mark.Id == markId),
                markOperatingConditions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByMarkId_ShouldReturnNull_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.markOperatingConditions);
            var repo = new SqlMarkOperatingConditionsRepo(context);

            // Act
            var markOperatingConditions = repo.GetByMarkId(999);

            // Assert
            Assert.Null(markOperatingConditions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddMarkOperatingConditions()
        {
            // Arrange
            var context = GetContext(TestData.markOperatingConditions);
            var repo = new SqlMarkOperatingConditionsRepo(context);

            int markId = 3;
            int envAggressivenessId = _rnd.Next(1, TestData.envAggressiveness.Count());
            int operatingAreaId = _rnd.Next(1, TestData.operatingAreas.Count());
            int gasGroupId = _rnd.Next(1, TestData.gasGroups.Count());
            int constructionMaterialId = _rnd.Next(1, TestData.constructionMaterials.Count());
            int paintworkTypeId = _rnd.Next(1, TestData.paintworkTypes.Count());
            int highTensileBoltsTypeId = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());
            var markOperatingConditions = new MarkOperatingConditions
            {
                Mark = TestData.marks.FirstOrDefault(v => v.Id == markId),
                SafetyCoeff = 1.0f,
                EnvAggressiveness = TestData.envAggressiveness.FirstOrDefault(v => v.Id == envAggressivenessId),
                Temperature = -34,
                OperatingArea = TestData.operatingAreas.FirstOrDefault(v => v.Id == operatingAreaId),
                GasGroup = TestData.gasGroups.FirstOrDefault(v => v.Id == gasGroupId),
                ConstructionMaterial = TestData.constructionMaterials.FirstOrDefault(v => v.Id == constructionMaterialId),
                PaintworkType = TestData.paintworkTypes.FirstOrDefault(v => v.Id == paintworkTypeId),
                HighTensileBoltsType = TestData.highTensileBoltsTypes.FirstOrDefault(v => v.Id == highTensileBoltsTypeId),
            };

            // Act
            repo.Add(markOperatingConditions);

            // Assert
            Assert.NotNull(repo.GetByMarkId(markId));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateMarkOperatingConditions()
        {
            // Arrange
            var markOperatingConditionsList = new List<MarkOperatingConditions> { };
            foreach (var moc in TestData.markOperatingConditions)
            {
                markOperatingConditionsList.Add(new MarkOperatingConditions
                {
                    Mark = moc.Mark,
                    SafetyCoeff = moc.SafetyCoeff,
                    EnvAggressiveness = moc.EnvAggressiveness,
                    Temperature = moc.Temperature,
                    OperatingArea = moc.OperatingArea,
                    GasGroup = moc.GasGroup,
                    ConstructionMaterial = moc.ConstructionMaterial,
                    PaintworkType = moc.PaintworkType,
                    HighTensileBoltsType = moc.HighTensileBoltsType,
                });
            }
            var context = GetContext(markOperatingConditionsList);
            var repo = new SqlMarkOperatingConditionsRepo(context);

            int markId = 1;
            var markOperatingConditions = markOperatingConditionsList.FirstOrDefault(v => v.Mark.Id == markId);
            markOperatingConditions.Temperature = -40;

            // Act
            repo.Update(markOperatingConditions);

            // Assert
            Assert.Equal(markOperatingConditions.Temperature, repo.GetByMarkId(markId).Temperature);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
