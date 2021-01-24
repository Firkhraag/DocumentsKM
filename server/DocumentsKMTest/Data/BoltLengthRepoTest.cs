using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class BoltLengthRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<BoltLength> boltLengths)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "BoltLengthTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.BoltDiameters.AddRange(TestData.boltDiameters);
            context.BoltLengths.AddRange(boltLengths);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByDiameterId_ShouldReturnBoltLengths()
        {
            // Arrange
            var context = GetContext(TestData.boltLengths);
            var repo = new SqlBoltLengthRepo(context);

            int diameterId = _rnd.Next(1, TestData.constructionTypes.Count());

            // Act
            var boltLengths = repo.GetAllByDiameterId(diameterId);

            // Assert
            Assert.Equal(TestData.boltLengths.Where(
                v => v.Diameter.Id == diameterId), boltLengths);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnBoltLength()
        {
            // Arrange
            var context = GetContext(TestData.boltLengths);
            var repo = new SqlBoltLengthRepo(context);

            int id = _rnd.Next(1, TestData.boltLengths.Count());

            // Act
            var boltLength = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.boltLengths.SingleOrDefault(
                v => v.Id == id), boltLength);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.boltLengths);
            var repo = new SqlBoltLengthRepo(context);

            int wrongId = 999;

            // Act
            var boltLength = repo.GetById(wrongId);

            // Assert
            Assert.Null(boltLength);

            context.Database.EnsureDeleted();
        }
    }
}
