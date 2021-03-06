using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class AttachedDocRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<AttachedDoc> attachedDocs)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "AttachedDocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Marks.AddRange(TestData.marks);
            context.AttachedDocs.AddRange(attachedDocs);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAttachedDocs()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var attachedDocs = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.attachedDocs.Where(v => v.Mark.Id == markId),
                attachedDocs);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            // Act
            var attachedDocs = repo.GetAllByMarkId(999);

            // Assert
            Assert.Empty(attachedDocs);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnAttachedDoc()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            int id = _rnd.Next(1, TestData.attachedDocs.Count());

            // Act
            var attachedDoc = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.attachedDocs.SingleOrDefault(v => v.Id == id),
                attachedDoc);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Act
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            var attachedDoc = repo.GetById(999);

            // Assert
            Assert.Null(attachedDoc);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnAttachedDoc()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            int id = _rnd.Next(1, TestData.attachedDocs.Count());
            var foundAttachedDoc = TestData.attachedDocs.FirstOrDefault(v => v.Id == id);
            var markId = foundAttachedDoc.Mark.Id;
            var designation = foundAttachedDoc.Designation;

            // Act
            var attachedDoc = repo.GetByUniqueKey(markId, designation);

            // Assert
            Assert.Equal(id, attachedDoc.Id);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            var markId = TestData.marks[0].Id;
            var designation = TestData.attachedDocs[0].Designation;

            // Act
            var attachedDoc1 = repo.GetByUniqueKey(999, designation);
            var attachedDoc2 = repo.GetByUniqueKey(markId, "NotFound");

            // Assert
            Assert.Null(attachedDoc1);
            Assert.Null(attachedDoc2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddAttachedDoc()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            var attachedDoc = new AttachedDoc
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                Designation = "NewCreate",
                Name = "NewCreate",
            };

            // Act
            repo.Add(attachedDoc);

            // Assert
            Assert.NotNull(repo.GetById(attachedDoc.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateAttachedDoc()
        {
            // Arrange
            var attachedDocs = new List<AttachedDoc> { };
            foreach (var ad in TestData.attachedDocs)
            {
                attachedDocs.Add(new AttachedDoc
                {
                    Id = ad.Id,
                    Mark = ad.Mark,
                    Designation = ad.Designation,
                    Name = ad.Name,
                });
            }
            var context = GetContext(attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            int id = _rnd.Next(1, attachedDocs.Count());
            var attachedDoc = attachedDocs.FirstOrDefault(v => v.Id == id);
            attachedDoc.Name = "NewUpdate";

            // Act
            repo.Update(attachedDoc);

            // Assert
            Assert.Equal(attachedDoc.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteAttachedDoc()
        {
            // Arrange
            var context = GetContext(TestData.attachedDocs);
            var repo = new SqlAttachedDocRepo(context);

            int id = _rnd.Next(1, TestData.attachedDocs.Count());
            var attachedDoc = TestData.attachedDocs.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(attachedDoc);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
