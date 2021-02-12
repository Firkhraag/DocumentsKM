using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class steelRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Steel> steel)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SteelTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Steel.AddRange(steel);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnsteel()
        {
            // Act
            var context = GetContext(TestData.steel);
            var repo = new SqlSteelRepo(context);

            var steel = repo.GetAll();

            // Assert
            Assert.Equal(TestData.steel, steel);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnsteel()
        {
            // Arrange
            var context = GetContext(TestData.steel);
            var repo = new SqlSteelRepo(context);

            int id = _rnd.Next(1, TestData.steel.Count());

            // Act
            var steel = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.steel.SingleOrDefault(v => v.Id == id),
                steel);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.steel);
            var repo = new SqlSteelRepo(context);

            // Act
            var steel = repo.GetById(999);

            // Assert
            Assert.Null(steel);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
