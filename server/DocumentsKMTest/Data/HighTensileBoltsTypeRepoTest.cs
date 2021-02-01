using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class HighTensileBoltsTypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<HighTensileBoltsType> highTensileBoltsTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "HighTensileBoltsTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.HighTensileBoltsTypes.AddRange(highTensileBoltsTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnHighTensileBoltsTypes()
        {
            // Arrange
            var context = GetContext(TestData.highTensileBoltsTypes);
            var repo = new SqlHighTensileBoltsTypeRepo(context);

            // Act
            var highTensileBoltsTypes = repo.GetAll();

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes, highTensileBoltsTypes);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnHighTensileBoltsType()
        {
            // Arrange
            var context = GetContext(TestData.highTensileBoltsTypes);
            var repo = new SqlHighTensileBoltsTypeRepo(context);

            int id = _rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            // Act
            var highTensileBoltsType = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes.SingleOrDefault(v => v.Id == id),
                highTensileBoltsType);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.highTensileBoltsTypes);
            var repo = new SqlHighTensileBoltsTypeRepo(context);

            // Act
            var highTensileBoltsType = repo.GetById(999);

            // Assert
            Assert.Null(highTensileBoltsType);

            context.Database.EnsureDeleted();
        }
    }
}
