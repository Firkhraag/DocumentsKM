using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class LinkedDocRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<LinkedDoc> linkedDocs)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "LinkedDocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.LinkedDocTypes.AddRange(TestData.linkedDocTypes);
            context.LinkedDocs.AddRange(linkedDocs);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByDocTypeId_ShouldReturnLinkedDocs()
        {
            // Arrange
            var context = GetContext(TestData.linkedDocs);
            var repo = new SqlLinkedDocRepo(context);

            var docTypeId = _rnd.Next(1, TestData.linkedDocTypes.Count());

            // Act
            var linkedDocs = repo.GetAllByDocTypeId(docTypeId);

            // Assert
            Assert.Equal(TestData.linkedDocs.Where(
                v => v.Type.Id == docTypeId), linkedDocs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByDocTypeId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.linkedDocs);
            var repo = new SqlLinkedDocRepo(context);

            var wrongDocTypeId = 999;

            // Act
            var linkedDocs = repo.GetAllByDocTypeId(wrongDocTypeId);

            // Assert
            Assert.Empty(linkedDocs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnLinkedDoc()
        {
            // Arrange
            var context = GetContext(TestData.linkedDocs);
            var repo = new SqlLinkedDocRepo(context);

            int id = _rnd.Next(1, TestData.linkedDocs.Count());

            // Act
            var linkedDoc = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.linkedDocs.SingleOrDefault(v => v.Id == id),
                linkedDoc);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.linkedDocs);
            var repo = new SqlLinkedDocRepo(context);

            int wrongId = 999;

            // Act
            var linkedDoc = repo.GetById(wrongId);

            // Assert
            Assert.Null(linkedDoc);

            context.Database.EnsureDeleted();
        }
    }
}
