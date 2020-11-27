using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class WeldingControlRepoTest : IDisposable
    {
        private readonly IWeldingControlRepo _repo;

        public WeldingControlRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "WeldingControlTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.WeldingControl.AddRange(TestData.weldingControl);
            context.SaveChanges();
            _repo = new SqlWeldingControlRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "WeldingControlTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllWeldingControl()
        {
            // Act
            var weldingControl = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.weldingControl, weldingControl);
        }

        [Fact]
        public void GetById_ShouldReturnWeldingControl()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.weldingControl.Count());

            // Act
            var weldingControl = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.weldingControl.SingleOrDefault(v => v.Id == id),
                weldingControl);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var weldingControl = _repo.GetById(wrongId);

            // Assert
            Assert.Null(weldingControl);
        }
    }
}
