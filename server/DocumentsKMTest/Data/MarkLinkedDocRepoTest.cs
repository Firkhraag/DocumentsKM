using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkLinkedDocRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<MarkLinkedDoc> markLinkedDocs)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkLinkedDocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.MarkLinkedDocs.AddRange(markLinkedDocs);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnMarkLinkedDocs()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var markLinkedDocs = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.markLinkedDocs.Where(v => v.Mark.Id == markId),
                markLinkedDocs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            var wrongMarkId = 999;

            // Act
            var markLinkedDocs = repo.GetAllByMarkId(wrongMarkId);

            // Assert
            Assert.Empty(markLinkedDocs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnMarkLinkedDoc()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            int id = _rnd.Next(1, TestData.markLinkedDocs.Count());

            // Act
            var markLinkedDoc = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.markLinkedDocs.SingleOrDefault(v => v.Id == id), markLinkedDoc);
            
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Act
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            var markLinkedDoc = repo.GetById(999);

            // Assert
            Assert.Null(markLinkedDoc);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByMarkIdAndLinkedDocId_ShouldReturnMarkLinkedDoc()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            int markId = _rnd.Next(1, _maxMarkId);
            int linkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());

            // Act
            var markLinkedDoc = repo.GetByMarkIdAndLinkedDocId(markId, linkedDocId);

            // Assert
            Assert.Equal(TestData.markLinkedDocs.SingleOrDefault(
                v => v.Mark.Id == markId && v.LinkedDoc.Id == linkedDocId), markLinkedDoc);
            
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetByMarkIdAndLinkedDocId_ShouldReturnNull()
        {
            // Act
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            var markLinkedDoc = repo.GetById(999);

            // Assert
            Assert.Null(markLinkedDoc);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddMarkLinkedDoc()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            int linkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            var markLinkedDoc = new MarkLinkedDoc
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                LinkedDoc = TestData.linkedDocs.SingleOrDefault(v => v.Id == linkedDocId),
            };

            // Act
            repo.Add(markLinkedDoc);

            // Assert
            Assert.NotEqual(0, markLinkedDoc.Id);
            Assert.Equal(
                TestData.markLinkedDocs.Where(v => v.Mark.Id == markId).Count() + 1,
                repo.GetAllByMarkId(markId).Count());

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Update_ShouldUpdateMarkLinkedDoc()
        {
            // Arrange
            var markLinkedDocs = new List<MarkLinkedDoc>{};
            foreach (var mld in TestData.markLinkedDocs)
            {
                markLinkedDocs.Add(new MarkLinkedDoc
                {
                    Id = mld.Id,
                    Mark = mld.Mark,
                    LinkedDoc = mld.LinkedDoc,
                });
            }
            var context = GetContext(markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            int id = _rnd.Next(1, markLinkedDocs.Count());
            var markLinkedDoc = markLinkedDocs.FirstOrDefault(v => v.Id == id);
            int linkedDocId = 1;
            markLinkedDoc.LinkedDoc = TestData.linkedDocs.FirstOrDefault(v => v.Id == linkedDocId);

            // Act
            repo.Update(markLinkedDoc);

            // Assert
            Assert.Equal(markLinkedDoc.LinkedDoc.Id, repo.GetById(id).LinkedDoc.Id);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteMarkLinkedDoc()
        {
            // Arrange
            var context = GetContext(TestData.markLinkedDocs);
            var repo = new SqlMarkLinkedDocRepo(context);

            int id = _rnd.Next(1, TestData.markLinkedDocs.Count());
            var markLinkedDoc = TestData.markLinkedDocs.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(markLinkedDoc);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
        }
    }
}
