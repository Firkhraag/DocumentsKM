using System;
using System.Linq;
using DocumentsKM.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentRepoTest : IDisposable
    {
        private readonly IDepartmentRepo _repo;

        public DepartmentRepoTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DepartmentTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Departments.AddRange(TestData.departments);
            context.SaveChanges();
            _repo = new SqlDepartmentRepo(context);
        }

        public void Dispose()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DepartmentTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAll_ShouldReturnAllDepartments()
        {
            // Act
            var departments = _repo.GetAll();

            // Assert
            Assert.Equal(TestData.departments, departments);
        }

        [Fact]
        public void GetById_ShouldReturnDepartment()
        {
            // Arrange
            var rnd = new Random();
            int id = rnd.Next(1, TestData.departments.Count());

            // Act
            var department = _repo.GetById(id);

            // Assert
            Assert.Equal(TestData.departments.SingleOrDefault(v => v.Id == id),
                department);
        }

        [Fact]
        public void GetById_ShouldReturnNull()
        {
            // Arrange
            int wrongId = 999;

            // Act
            var department = _repo.GetById(wrongId);

            // Assert
            Assert.Null(department);
        }
    }
}
