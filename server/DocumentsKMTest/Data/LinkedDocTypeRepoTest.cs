using System.Collections.Generic;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class LinkedDocTypeRepoTest
    {
        private ApplicationContext GetContext(List<LinkedDocType> linkedDocTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "LinkedDocTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.LinkedDocTypes.AddRange(linkedDocTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnLinkedDocTypes()
        {
            // Arrange
            var context = GetContext(TestData.linkedDocTypes);
            var repo = new SqlLinkedDocTypeRepo(context);

            // Act
            var linkedDocTypes = repo.GetAll();

            // Assert
            Assert.Equal(TestData.linkedDocTypes, linkedDocTypes);

            context.Database.EnsureDeleted();
        }
    }
}
