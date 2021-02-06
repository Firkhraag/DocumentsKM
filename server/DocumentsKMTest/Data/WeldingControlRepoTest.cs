using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class WeldingControlRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<WeldingControl> weldingControl)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "WeldingControlTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.WeldingControl.AddRange(weldingControl);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnWeldingControl()
        {
            // Act
            var context = GetContext(TestData.weldingControl);
            var repo = new SqlWeldingControlRepo(context);

            var weldingControl = repo.GetAll();

            // Assert
            Assert.Equal(TestData.weldingControl, weldingControl);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnWeldingControl()
        {
            // Arrange
            var context = GetContext(TestData.weldingControl);
            var repo = new SqlWeldingControlRepo(context);

            int id = _rnd.Next(1, TestData.weldingControl.Count());

            // Act
            var weldingControl = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.weldingControl.SingleOrDefault(v => v.Id == id),
                weldingControl);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.weldingControl);
            var repo = new SqlWeldingControlRepo(context);

            // Act
            var weldingControl = repo.GetById(999);

            // Assert
            Assert.Null(weldingControl);

            context.Database.EnsureDeleted();
        }
    }
}
