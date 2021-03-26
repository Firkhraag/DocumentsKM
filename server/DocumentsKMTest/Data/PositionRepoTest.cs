using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PositionRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Position> positions)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "PositionTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Positions.AddRange(positions);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnPositions()
        {
            // Act
            var context = GetContext(TestData.positions);
            var repo = new SqlPositionRepo(context);

            var positions = repo.GetAll();

            // Assert
            Assert.Equal(TestData.positions, positions);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnPosition()
        {
            // Arrange
            var context = GetContext(TestData.positions);
            var repo = new SqlPositionRepo(context);

            int id = _rnd.Next(1, TestData.positions.Count());

            // Act
            var position = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.positions.SingleOrDefault(v => v.Id == id),
                position);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.positions);
            var repo = new SqlPositionRepo(context);

            // Act
            var position = repo.GetById(999);

            // Assert
            Assert.Null(position);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
