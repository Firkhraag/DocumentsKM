using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionMaterialRepoTest : IDisposable
    {
        private readonly IConstructionMaterialRepo _repo;

        public ConstructionMaterialRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionMaterialTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.ConstructionMaterials.AddRange(TestData.constructionMaterials);
            context.SaveChanges();
            _repo = new SqlConstructionMaterialRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionMaterialTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllConstructionMaterials()
        {
            // Act
            var constructionMaterials = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.constructionMaterials, constructionMaterials);
        }

        [Fact]
        public void GetById_ShouldReturnConstructionMaterial()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.constructionMaterials.Count());

            // Act
            var constructionMaterial = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.constructionMaterials.SingleOrDefault(v => v.Id == id),
                constructionMaterial);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var constructionMaterial = _repo.GetById(wrongId);

            // Assert
            Assert.Null(constructionMaterial);
        }
    }
}
