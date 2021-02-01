using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GasGroupRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<GasGroup> gasGroups)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "GasGroupTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.GasGroups.AddRange(gasGroups);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnGasGroups()
        {
            // Arrange
            var context = GetContext(TestData.gasGroups);
            var repo = new SqlGasGroupRepo(context);

            // Act
            var gasGroups = repo.GetAll();

            // Assert
            Assert.Equal(TestData.gasGroups, gasGroups);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnGasGroup()
        {
            // Arrange
            var context = GetContext(TestData.gasGroups);
            var repo = new SqlGasGroupRepo(context);

            int id = _rnd.Next(1, TestData.gasGroups.Count());

            // Act
            var gasGroup = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.gasGroups.SingleOrDefault(v => v.Id == id),
                gasGroup);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.gasGroups);
            var repo = new SqlGasGroupRepo(context);

            // Act
            var gasGroup = repo.GetById(999);

            // Assert
            Assert.Null(gasGroup);

            context.Database.EnsureDeleted();
        }
    }
}
