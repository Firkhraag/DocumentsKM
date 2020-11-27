using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GasGroupRepoTest : IDisposable
    {
        private readonly IGasGroupRepo _repo;

        public GasGroupRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GasGroupTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.GasGroups.AddRange(TestData.gasGroups);
            context.SaveChanges();
            _repo = new SqlGasGroupRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GasGroupTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllGasGroups()
        {
            // Act
            var gasGroups = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.gasGroups, gasGroups);
        }

        [Fact]
        public void GetById_ShouldReturnGasGroup()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.gasGroups.Count());

            // Act
            var gasGroup = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.gasGroups.SingleOrDefault(v => v.Id == id),
                gasGroup);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var gasGroup = _repo.GetById(wrongId);

            // Assert
            Assert.Null(gasGroup);
        }
    }
}
