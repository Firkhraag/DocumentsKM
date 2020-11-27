using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PaintworkTypeRepoTest : IDisposable
    {
        private readonly IPaintworkTypeRepo _repo;

        public PaintworkTypeRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "PaintworkTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.PaintworkTypes.AddRange(TestData.paintworkTypes);
            context.SaveChanges();
            _repo = new SqlPaintworkTypeRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "PaintworkTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllPaintworkTypes()
        {
            // Act
            var paintworkTypes = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.paintworkTypes, paintworkTypes);
        }

        [Fact]
        public void GetById_ShouldReturnPaintworkType()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.paintworkTypes.Count());

            // Act
            var paintworkType = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.paintworkTypes.SingleOrDefault(v => v.Id == id),
                paintworkType);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var paintworkType = _repo.GetById(wrongId);

            // Assert
            Assert.Null(paintworkType);
        }
    }
}
