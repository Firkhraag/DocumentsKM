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
            Assert.Equal(TestData.departments, departments);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllActive_ShouldReturnDepartments()
        {
            // Act
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            var departments = repo.GetAllActive();

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

        [Fact]
        public void Add_ShouldAddDepartment()
        {
            // Arrange
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            var department = new Department
            {
                Code = "NewCreate",
                Name = "NewCreate",
                ShortName = "NewCreate",
                IsActive = true,
            };

            // Act
            repo.Add(department);

            // Assert
            Assert.NotNull(repo.GetById(department.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateDepartment()
        {
            // Arrange
            var departments = new List<Department> { };
            foreach (var d in TestData.departments)
            {
                departments.Add(new Department
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    IsActive = d.IsActive,
                });
            }
            var context = GetContext(departments);
            var repo = new SqlDepartmentRepo(context);

            int id = _rnd.Next(1, departments.Count());
            var department = departments.FirstOrDefault(v => v.Id == id);
            department.Name = "NewUpdate";

            // Act
            repo.Update(department);

            // Assert
            Assert.Equal(department.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteDepartment()
        {
            // Arrange
            var context = GetContext(TestData.departments);
            var repo = new SqlDepartmentRepo(context);

            int id = _rnd.Next(1, TestData.departments.Count());
            var department = TestData.departments.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(department);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
