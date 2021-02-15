using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class AdditionalWorkRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<AdditionalWork> additionalWork)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "AdditionalWorkTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Marks.AddRange(TestData.marks);
            context.Employees.AddRange(TestData.employees);
            context.AdditionalWork.AddRange(additionalWork);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAdditionalWork()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var additionalWork = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.additionalWork.Where(
                v => v.Mark.Id == markId), additionalWork);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            // Act
            var additionalWork = repo.GetAllByMarkId(999);

            // Assert
            Assert.Empty(additionalWork);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnDoc()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            int id = _rnd.Next(1, TestData.additionalWork.Count());

            // Act
            var additionalWork = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.additionalWork.SingleOrDefault(
                v => v.Id == id), additionalWork);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            // Act
            var additionalWork = repo.GetById(999);

            // Assert
            Assert.Null(additionalWork);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnDoc()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            var markId = TestData.additionalWork[0].Mark.Id;
            var employeeId = TestData.additionalWork[0].Employee.Id;

            // Act
            var additionalWork = repo.GetByUniqueKey(markId, employeeId);

            // Assert
            Assert.Equal(TestData.additionalWork[0], additionalWork);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByUniqueKey_ShouldReturnNull_WhenWrongKey()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            var markId = TestData.additionalWork[0].Mark.Id;
            var employeeId = TestData.additionalWork[0].Employee.Id;

            // Act
            var additionalWork1 = repo.GetByUniqueKey(999, employeeId);
            var additionalWork2 = repo.GetByUniqueKey(markId, 999);

            // Assert
            Assert.Null(additionalWork1);
            Assert.Null(additionalWork2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddDoc()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            int employeeId = _rnd.Next(1, TestData.employees.Count());
            var additionalWork = new AdditionalWork
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                Employee = TestData.employees.SingleOrDefault(v => v.Id == employeeId),
                Valuation = 1,
                MetalOrder = 1,
            };

            // Act
            repo.Add(additionalWork);

            // Assert
            Assert.NotNull(repo.GetById(additionalWork.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateDoc()
        {
            // Arrange
            var additionalWorkArr = new List<AdditionalWork> { };
            foreach (var w in TestData.additionalWork)
            {
                additionalWorkArr.Add(new AdditionalWork
                {
                    Id = w.Id,
                    Mark = w.Mark,
                    Employee = w.Employee,
                    Valuation = w.Valuation,
                    MetalOrder = w.MetalOrder,
                });
            }
            var context = GetContext(additionalWorkArr);
            var repo = new SqlAdditionalWorkRepo(context);

            int id = _rnd.Next(1, additionalWorkArr.Count());
            var additionalWork = additionalWorkArr.FirstOrDefault(v => v.Id == id);
            additionalWork.Valuation = 99;

            // Act
            repo.Update(additionalWork);

            // Assert
            Assert.Equal(additionalWork.Valuation, repo.GetById(id).Valuation);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteDoc()
        {
            // Arrange
            var context = GetContext(TestData.additionalWork);
            var repo = new SqlAdditionalWorkRepo(context);

            int id = _rnd.Next(1, TestData.additionalWork.Count());
            var additionalWork = TestData.additionalWork.FirstOrDefault(
                v => v.Id == id);

            // Act
            repo.Delete(additionalWork);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
