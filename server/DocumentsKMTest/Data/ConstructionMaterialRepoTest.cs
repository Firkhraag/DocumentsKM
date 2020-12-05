using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionMaterialRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<ConstructionMaterial> constructionMaterials)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionMaterialTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.ConstructionMaterials.AddRange(constructionMaterials);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnConstructionMaterials()
        {
            // Arrange
            var context = GetContext(TestData.constructionMaterials);
            var repo = new SqlConstructionMaterialRepo(context);

            // Act
            var constructionMaterials = repo.GetAll();

            // Assert
            Assert.Equal(TestData.constructionMaterials, constructionMaterials);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnConstructionMaterial()
        {
            // Arrange
            var context = GetContext(TestData.constructionMaterials);
            var repo = new SqlConstructionMaterialRepo(context);

            int id = _rnd.Next(1, TestData.constructionMaterials.Count());

            // Act
            var constructionMaterial = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.constructionMaterials.SingleOrDefault(v => v.Id == id),
                constructionMaterial);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.constructionMaterials);
            var repo = new SqlConstructionMaterialRepo(context);

            int wrongId = 999;

            // Act
            var constructionMaterial = repo.GetById(wrongId);

            // Assert
            Assert.Null(constructionMaterial);

            context.Database.EnsureDeleted();
        }
    }
}
