using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<Doc> docs)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.DocTypes.AddRange(TestData.docTypes);
            context.Marks.AddRange(TestData.marks);
            context.Employees.AddRange(TestData.employees);
            context.Docs.AddRange(docs);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnDocs()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var docs = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.docs.Where(v => v.Mark.Id == markId), docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var wrongMarkId = 999;

            // Act
            var docs = repo.GetAllByMarkId(wrongMarkId);

            // Assert
            Assert.Empty(docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkIdAndDocType_ShouldReturnDocs()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);
            var docTypeId = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docs = repo.GetAllByMarkIdAndDocType(markId, docTypeId);

            // Assert
            Assert.Equal(TestData.docs.Where(v => v.Mark.Id == markId && v.Type.Id == docTypeId), docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkIdAndDocType_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var wrongMarkId = 999;
            var docTypeId = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docs = repo.GetAllByMarkIdAndDocType(wrongMarkId, docTypeId);

            // Assert
            Assert.Empty(docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkIdAndNotDocType_ShouldReturnDocs()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);
            var docTypeId = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docs = repo.GetAllByMarkIdAndNotDocType(markId, docTypeId);

            // Assert
            Assert.Equal(TestData.docs.Where(v => v.Mark.Id == markId && v.Type.Id != docTypeId), docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkIdAndNotDocType_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            var wrongMarkId = 999;
            var docTypeId = _rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docs = repo.GetAllByMarkIdAndNotDocType(wrongMarkId, docTypeId);

            // Assert
            Assert.Empty(docs);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnDoc()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            int id = _rnd.Next(1, TestData.docs.Count());

            // Act
            var doc = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.docs.SingleOrDefault(v => v.Id == id), doc);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            // Act
            var doc = repo.GetById(999);

            // Assert
            Assert.Null(doc);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddDoc()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            int docTypeId = _rnd.Next(1, TestData.docTypes.Count());
            int creatorId = _rnd.Next(1, TestData.employees.Count());
            int inspectorId = _rnd.Next(1, TestData.employees.Count());
            int normContrId = _rnd.Next(1, TestData.employees.Count());
            var doc = new Doc
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                Type = TestData.docTypes.SingleOrDefault(v => v.Id == docTypeId),
                Name = "NewCreate",
                Form = 1.0f,
                Creator = TestData.employees.SingleOrDefault(v => v.Id == creatorId),
                Inspector = TestData.employees.SingleOrDefault(v => v.Id == inspectorId),
                NormContr = TestData.employees.SingleOrDefault(v => v.Id == normContrId),
                ReleaseNum = 1,
                NumOfPages = 1,
            };

            // Act
            repo.Add(doc);

            // Assert
            Assert.NotEqual(0, doc.Id);
            Assert.Equal(
                TestData.docs.Where(v => v.Mark.Id == markId).Count() + 1,
                repo.GetAllByMarkId(markId).Count());

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Update_ShouldUpdateDoc()
        {
            // Arrange
            var docs = new List<Doc>{};
            foreach (var d in TestData.docs)
            {
                docs.Add(new Doc
                {
                    Id = d.Id,
                    Mark = d.Mark,
                    Type = d.Type,
                    Name = d.Name,
                    Form = d.Form,
                    Creator = d.Creator,
                    Inspector = d.Inspector,
                    NormContr = d.NormContr,
                    ReleaseNum = d.ReleaseNum,
                    NumOfPages = d.NumOfPages,
                    Note = d.Note,
                });
            }
            var context = GetContext(docs);
            var repo = new SqlDocRepo(context);

            int id = _rnd.Next(1, docs.Count());
            var doc = docs.FirstOrDefault(v => v.Id == id);
            doc.Name = "NewUpdate";

            // Act
            repo.Update(doc);

            // Assert
            Assert.Equal(doc.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteDoc()
        {
            // Arrange
            var context = GetContext(TestData.docs);
            var repo = new SqlDocRepo(context);

            int id = _rnd.Next(1, TestData.docs.Count());
            var doc = TestData.docs.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(doc);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
        }
    }
}
