using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class OperatingAreaRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<OperatingArea> operatingAreas)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "OperatingAreaTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.OperatingAreas.AddRange(operatingAreas);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnOperatingAreas()
        {
            // Arrange
            var context = GetContext(TestData.operatingAreas);
            var repo = new SqlOperatingAreaRepo(context);

            // Act
            var operatingAreas = repo.GetAll();

            // Assert
            Assert.Equal(TestData.operatingAreas, operatingAreas);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnOperatingArea()
        {
            // Arrange
            var context = GetContext(TestData.operatingAreas);
            var repo = new SqlOperatingAreaRepo(context);

            int id = _rnd.Next(1, TestData.operatingAreas.Count());

            // Act
            var operatingArea = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.operatingAreas.SingleOrDefault(v => v.Id == id),
                operatingArea);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.operatingAreas);
            var repo = new SqlOperatingAreaRepo(context);

            // Act
            var operatingArea = repo.GetById(999);

            // Assert
            Assert.Null(operatingArea);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
