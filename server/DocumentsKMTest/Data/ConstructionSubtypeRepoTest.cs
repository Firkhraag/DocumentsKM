using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionSubtypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<ConstructionSubtype> constructionSubtypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "ConstructionSubtypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Projects.AddRange(TestData.projects);
            context.ConstructionSubtypes.AddRange(constructionSubtypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByTypeId_ShouldReturnConstructionSubtypes()
        {
            // Arrange
            var context = GetContext(TestData.constructionSubtypes);
            var repo = new SqlConstructionSubtypeRepo(context);

            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());

            // Act
            var constructionSubtypes = repo.GetAllByTypeId(typeId);

            // Assert
            Assert.Equal(TestData.constructionSubtypes.Where(
                v => v.Type.Id == typeId), constructionSubtypes);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnConstructionSubtype()
        {
            // Arrange
            var context = GetContext(TestData.constructionSubtypes);
            var repo = new SqlConstructionSubtypeRepo(context);

            int id = _rnd.Next(1, TestData.constructionSubtypes.Count());

            // Act
            var constructionSubtype = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.constructionSubtypes.SingleOrDefault(
                v => v.Id == id), constructionSubtype);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.constructionSubtypes);
            var repo = new SqlConstructionSubtypeRepo(context);

            int wrongId = 999;

            // Act
            var ConstructionSubtype = repo.GetById(wrongId);

            // Assert
            Assert.Null(ConstructionSubtype);

            context.Database.EnsureDeleted();
        }
    }
}
