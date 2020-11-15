using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class AttachedDocRepoTest
    {
        // public void Add_WhenHaveNoEmail()
        // {
        //     IPersonRepository sut = GetInMemoryPersonRepository();
        //     Person person = new Person()
        //     {
        //         PersonId = 1,
        //         FirstName = "fred",
        //         Surname = "Blogs"
        //     };

        //     Person savedPerson = sut.Add(person);

        //     Assert.Equal(1, sut.GetAll().Count());
        //     Assert.Equal("fred", savedPerson.FirstName);
        //     Assert.Equal("Blogs", savedPerson.Surname);
        //     Assert.Null(savedPerson.EmailAddresses);
        // }

        private readonly IAttachedDocRepo _repo;

        public AttachedDocRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "AttachedDocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AttachedDocs.AddRange(TestData.attachedDocs);
            context.SaveChanges();

            _repo = new SqlAttachedDocRepo(context);
        }

        ~AttachedDocRepoTest()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "AttachedDocTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAllAttachedDocsByMarkId()
        {
            // Arrange
            var id = TestData.marks[0].Id;

            // Act
            var attachedDocs = _repo.GetAllByMarkId(TestData.marks[0].Id);

            // Assert
            Assert.Equal(TestData.attachedDocs.Where(v => v.Mark.Id == id),
                attachedDocs);
        }

        [Fact]
        public void GetById_ShouldReturnAttachedDoc()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.attachedDocs.Count());

            // Act
            var attachedDoc = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.attachedDocs.SingleOrDefault(v => v.Id == id),
                attachedDoc);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Act
            var attachedDoc = _repo.GetById(999);

            // Assert
            Assert.Null(attachedDoc);
        }

        [Fact]
        public void GetByUniqueKeyValues_ShouldReturnAttachedDoc()
        {
            // Arrange
            var markId = TestData.marks[0].Id;
            var designation = TestData.attachedDocs[0].Designation;

            // Act
            var attachedDoc = _repo.GetByUniqueKeyValues(markId, designation);

            // Assert
            Assert.Equal(TestData.attachedDocs.SingleOrDefault(
                v => v.Mark.Id == markId && v.Designation == designation), attachedDoc);
        }

        [Fact]
        public void GetByUniqueKeyValues_ShouldReturnNull()
        {
            // Arrange
            var markId = TestData.marks[0].Id;
            var wrongMarkId = 999;
            var designation = TestData.attachedDocs[0].Designation;
            var wrongDesignation = "NotFound";

            // Act
            var attachedDoc1 = _repo.GetByUniqueKeyValues(wrongMarkId, designation);
            var attachedDoc2 = _repo.GetByUniqueKeyValues(markId, wrongDesignation);

            // Assert
            Assert.Null(attachedDoc1);
            Assert.Null(attachedDoc2);
        }
    }
}

        // public void Add(AttachedDoc attachedDoc)
        // {
        //     _context.AttachedDocs.Add(attachedDoc);
        //     _context.SaveChanges();
        // }

        // public void Update(AttachedDoc attachedDoc)
        // {
        //     _context.Entry(attachedDoc).State = EntityState.Modified;
        //     _context.SaveChanges();
        // }

        // public void Delete(AttachedDoc attachedDoc)
        // {
        //     _context.AttachedDocs.Remove(attachedDoc);
        //     _context.SaveChanges();
        // }
