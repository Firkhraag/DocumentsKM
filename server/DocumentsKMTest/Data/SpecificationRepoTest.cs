using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SpecificationRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<Specification> specifications)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "SpecificationTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Specifications.AddRange(specifications);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnSpecifications()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var specifications = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.specifications.Where(v => v.Mark.Id == markId), specifications);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            // Act
            var specifications = repo.GetAllByMarkId(999);

            // Assert
            Assert.Empty(specifications);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnSpecification()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            int id = _rnd.Next(1, TestData.specifications.Count());

            // Act
            var specification = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.specifications.SingleOrDefault(v => v.Id == id),
                specification);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            // Act
            var specification = repo.GetById(999);

            // Assert
            Assert.Null(specification);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddSpecification()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            var specification = new Specification
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                Num = 42,
                IsCurrent = false,
            };

            // Act
            repo.Add(specification);

            // Assert
            Assert.NotNull(repo.GetById(specification.Id));

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Update_ShouldUpdateSpecification()
        {
            // Arrange
            var specifications = new List<Specification> { };
            foreach (var s in TestData.specifications)
            {
                specifications.Add(new Specification
                {
                    Id = s.Id,
                    Mark = s.Mark,
                    Num = s.Num,
                    IsCurrent = s.IsCurrent,
                });
            }
            var context = GetContext(specifications);
            var repo = new SqlSpecificationRepo(context);

            int id = _rnd.Next(1, specifications.Count());
            var specification = specifications.FirstOrDefault(v => v.Id == id);
            specification.Note = "NewUpdate";

            // Act
            repo.Update(specification);

            // Assert
            Assert.Equal(specification.Note, repo.GetById(id).Note);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteSpecification()
        {
            // Arrange
            var context = GetContext(TestData.specifications);
            var repo = new SqlSpecificationRepo(context);

            int id = _rnd.Next(1, TestData.specifications.Count());
            var specification = TestData.specifications.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(specification);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
        }
    }
}
