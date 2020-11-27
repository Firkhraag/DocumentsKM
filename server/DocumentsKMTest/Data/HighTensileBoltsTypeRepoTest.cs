using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class HighTensileBoltsTypeRepoTest : IDisposable
    {
        private readonly IHighTensileBoltsTypeRepo _repo;

        public HighTensileBoltsTypeRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "HighTensileBoltsTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.HighTensileBoltsTypes.AddRange(TestData.highTensileBoltsTypes);
            context.SaveChanges();
            _repo = new SqlHighTensileBoltsTypeRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "HighTensileBoltsTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllHighTensileBoltsTypes()
        {
            // Act
            var highTensileBoltsTypes = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes, highTensileBoltsTypes);
        }

        [Fact]
        public void GetById_ShouldReturnHighTensileBoltsType()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.highTensileBoltsTypes.Count());

            // Act
            var highTensileBoltsType = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes.SingleOrDefault(v => v.Id == id),
                highTensileBoltsType);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var highTensileBoltsType = _repo.GetById(wrongId);

            // Assert
            Assert.Null(highTensileBoltsType);
        }
    }
}
