using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SheetNameRepoTest : IDisposable
    {
        private readonly ISheetNameRepo _repo;

        public SheetNameRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SheetNameTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.SheetNames.AddRange(TestData.sheetNames);
            context.SaveChanges();
            _repo = new SqlSheetNameRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SheetNameTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllSheetNames()
        {
            // Act
            var sheetNames = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.sheetNames, sheetNames);
        }
    }
}
