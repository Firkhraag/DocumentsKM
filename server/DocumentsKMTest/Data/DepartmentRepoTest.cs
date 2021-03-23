using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Department> departments)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "DepartmentTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Departments.AddRange(departments);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnDepartments()
        {
            // Act
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            var departments = repo.GetAll();

            // Assert
            Assert.Equal(TestData.departments.Where(v => v.IsActive), departments);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnDepartment()
        {
            // Arrange
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            int id = _rnd.Next(1, TestData.departments.Count());

            // Act
            var department = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.departments.SingleOrDefault(v => v.Id == id),
                department);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            // Act
            var department = repo.GetById(999);

            // Assert
            Assert.Null(department);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
