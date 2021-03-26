using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DefaultValuesRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<DefaultValues> defaultValues)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DefaultValuesTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.DefaultValues.AddRange(defaultValues);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetByUserId_ShouldReturnDefaultValues()
        {
            // Arrange
            var context = GetContext(TestData.defaultValues);
            var repo = new SqlDefaultValuesRepo(context);

            int userId = _rnd.Next(1, TestData.users.Count());

            // Act
            var defaultValues = repo.GetByUserId(userId);

            // Assert
            Assert.Equal(TestData.defaultValues.SingleOrDefault(
                v => v.UserId == userId), defaultValues);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUserId_ShouldReturnNull_WhenWrongUserId()
        {
            // Arrange
            var context = GetContext(TestData.defaultValues);
            var repo = new SqlDefaultValuesRepo(context);

            // Act
            var defaultValues = repo.GetByUserId(999);

            // Assert
            Assert.Null(defaultValues);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
