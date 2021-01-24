using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GeneralDataSectionRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<GeneralDataSection> generalDataSections)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GeneralDataSectionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.GeneralDataSections.AddRange(generalDataSections);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnGeneralDataSections()
        {
            // Assert
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            // Act
            var generalDataSections = repo.GetAll();

            // Assert
            Assert.Equal(TestData.generalDataSections, generalDataSections);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnGeneralDataSection()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int id = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var generalDataSection = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.generalDataSections.SingleOrDefault(v => v.Id == id),
                generalDataSection);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.generalDataSections);
            var repo = new SqlGeneralDataSectionRepo(context);

            int wrongId = 999;

            // Act
            var generalDataSection = repo.GetById(wrongId);

            // Assert
            Assert.Null(generalDataSection);

            context.Database.EnsureDeleted();
        }
    }
}
