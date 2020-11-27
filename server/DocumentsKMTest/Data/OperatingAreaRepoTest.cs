using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class OperatingAreaRepoTest : IDisposable
    {
        private readonly IOperatingAreaRepo _repo;

        public OperatingAreaRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "OperatingAreaTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.OperatingAreas.AddRange(TestData.operatingAreas);
            context.SaveChanges();
            _repo = new SqlOperatingAreaRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "OperatingAreaTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllOperatingAreas()
        {
            // Act
            var operatingAreas = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.operatingAreas, operatingAreas);
        }

        [Fact]
        public void GetById_ShouldReturnOperatingArea()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.operatingAreas.Count());

            // Act
            var operatingArea = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.operatingAreas.SingleOrDefault(v => v.Id == id),
                operatingArea);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var operatingArea = _repo.GetById(wrongId);

            // Assert
            Assert.Null(operatingArea);
        }
    }
}
