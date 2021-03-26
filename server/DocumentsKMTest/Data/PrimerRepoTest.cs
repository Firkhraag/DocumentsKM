using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PrimerRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Primer> primer)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "PrimerTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Primer.AddRange(primer);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetByGroup_ShouldReturnPrimer()
        {
            // Arrange
            var context = GetContext(TestData.primer);
            var repo = new SqlPrimerRepo(context);

            int group = 1;

            // Act
            var primer = repo.GetByGroup(group);

            // Assert
            Assert.Equal(TestData.primer.OrderByDescending(
                v => v.Priority).FirstOrDefault(v =>
                    v.GroupNum == group), primer);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByGroup_ShouldReturnNull_WhenWrongGroup()
        {
            // Arrange
            var context = GetContext(TestData.primer);
            var repo = new SqlPrimerRepo(context);

            // Act
            var primer = repo.GetByGroup(999);

            // Assert
            Assert.Null(primer);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
