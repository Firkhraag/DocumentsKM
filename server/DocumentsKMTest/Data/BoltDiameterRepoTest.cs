using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class BoltDiameterRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<BoltDiameter> boltDiameters)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "BoltDiameterTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.BoltDiameters.AddRange(boltDiameters);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnBoltDiameters()
        {
            // Arrange
            var context = GetContext(TestData.boltDiameters);
            var repo = new SqlBoltDiameterRepo(context);

            // Act
            var boltDiameters = repo.GetAll();

            // Assert
            Assert.Equal(TestData.boltDiameters, boltDiameters);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnBoltDiameter()
        {
            // Arrange
            var context = GetContext(TestData.boltDiameters);
            var repo = new SqlBoltDiameterRepo(context);

            int id = _rnd.Next(1, TestData.boltDiameters.Count());

            // Act
            var boltDiameter = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.boltDiameters.SingleOrDefault(v => v.Id == id),
                boltDiameter);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.boltDiameters);
            var repo = new SqlBoltDiameterRepo(context);

            // Act
            var boltDiameter = repo.GetById(999);

            // Assert
            Assert.Null(boltDiameter);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
