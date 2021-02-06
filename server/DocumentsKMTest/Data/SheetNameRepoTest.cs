using System;
using System.Collections.Generic;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SheetNameRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<SheetName> sheetNames)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SheetNameTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SheetNames.AddRange(sheetNames);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnSheetNames()
        {
            // Arrange
            var context = GetContext(TestData.sheetNames);
            var repo = new SqlSheetNameRepo(context);

            // Act
            var sheetNames = repo.GetAll();

            // Assert
            Assert.Equal(TestData.sheetNames, sheetNames);

            context.Database.EnsureDeleted();
        }
    }
}
