using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocTypeRepoTest : IDisposable
    {
        private readonly IDocTypeRepo _repo;

        public DocTypeRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DocTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.DocTypes.AddRange(TestData.docTypes);
            context.SaveChanges();
            _repo = new SqlDocTypeRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DocTypeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllExceptId_ShouldReturnAllDocTypesExceptGivenId()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docTypes = _repo.GetAllExceptId(id);

            // Assert
            Assert.Equal(TestData.docTypes.Where(v => v.Id != id),
                docTypes);
        }

        [Fact]
        public void GetById_ShouldReturnDocType()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.docTypes.Count());

            // Act
            var docType = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.docTypes.SingleOrDefault(v => v.Id == id),
                docType);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var docType = _repo.GetById(wrongId);

            // Assert
            Assert.Null(docType);
        }
    }
}
