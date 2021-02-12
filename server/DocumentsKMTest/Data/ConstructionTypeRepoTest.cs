using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionTypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<ConstructionType> constructionTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ConstructionTypes.AddRange(constructionTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnConstructionTypes()
        {
            // Arrange
            var context = GetContext(TestData.constructionTypes);
            var repo = new SqlConstructionTypeRepo(context);

            // Act
            var constructionTypes = repo.GetAll();

            // Assert
            Assert.Equal(TestData.constructionTypes, constructionTypes);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnConstructionType()
        {
            // Arrange
            var context = GetContext(TestData.constructionTypes);
            var repo = new SqlConstructionTypeRepo(context);

            int id = _rnd.Next(1, TestData.constructionTypes.Count());

            // Act
            var constructionType = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.constructionTypes.SingleOrDefault(v => v.Id == id),
                constructionType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.constructionTypes);
            var repo = new SqlConstructionTypeRepo(context);

            // Act
            var constructionType = repo.GetById(999);

            // Assert
            Assert.Null(constructionType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
