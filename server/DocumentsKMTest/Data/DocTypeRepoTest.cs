using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocTypeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<DocType> docTypes)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DocTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.DocTypes.AddRange(docTypes);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllExceptId_ShouldReturnDocTypes()
        {
            // Arrange
            var context = GetContext(TestData.docTypes);
            var repo = new SqlDocTypeRepo(context);

            int id = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docTypes = repo.GetAllExceptId(id);

            // Assert
            Assert.Equal(TestData.docTypes.Where(v => v.Id != id),
                docTypes);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnDocType()
        {
            // Arrange
            var context = GetContext(TestData.docTypes);
            var repo = new SqlDocTypeRepo(context);

            int id = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docType = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.docTypes.SingleOrDefault(v => v.Id == id),
                docType);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.docTypes);
            var repo = new SqlDocTypeRepo(context);

            // Act
            var docType = repo.GetById(999);

            // Assert
            Assert.Null(docType);

            context.Database.EnsureDeleted();
        }
    }
}
