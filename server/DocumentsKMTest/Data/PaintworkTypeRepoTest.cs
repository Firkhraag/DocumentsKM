using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PaintworkTypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<PaintworkType> paintworkTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "PaintworkTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.PaintworkTypes.AddRange(paintworkTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnPaintworkTypes()
        {
            // Arrange
            var context = GetContext(TestData.paintworkTypes);
            var repo = new SqlPaintworkTypeRepo(context);

            // Act
            var paintworkTypes = repo.GetAll();

            // Assert
            Assert.Equal(TestData.paintworkTypes, paintworkTypes);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnPaintworkType()
        {
            // Arrange
            var context = GetContext(TestData.paintworkTypes);
            var repo = new SqlPaintworkTypeRepo(context);

            int id = _rnd.Next(1, TestData.paintworkTypes.Count());

            // Act
            var paintworkType = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.paintworkTypes.SingleOrDefault(v => v.Id == id),
                paintworkType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.paintworkTypes);
            var repo = new SqlPaintworkTypeRepo(context);

            // Act
            var paintworkType = repo.GetById(999);

            // Assert
            Assert.Null(paintworkType);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
