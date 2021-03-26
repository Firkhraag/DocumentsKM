using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EmployeeRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<Employee> employees)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "EmployeeTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Employees.AddRange(employees);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnEmployees()
        {
            // Act
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var employees = repo.GetAll();

            // Assert
            Assert.Equal(TestData.employees, employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllActive_ShouldReturnEmployees()
        {
            // Act
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var employees = repo.GetAllActive();

            // Assert
            Assert.Equal(TestData.employees.OrderBy(
                v => v.Position.Id).Where(v => v.IsActive), employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByDepartmentId_ShouldReturnEmployees()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var departmentId = _rnd.Next(1, TestData.departments.Count());

            // Act
            var employees = repo.GetAllByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && v.IsActive), employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            // Act
            var employees = repo.GetAllByDepartmentId(999);

            // Assert
            Assert.Empty(employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByDepartmentIdAndPosition_ShouldReturnEmployees()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var departmentId = _rnd.Next(1, TestData.departments.Count());
            var positionId = _rnd.Next(1, TestData.positions.Count());

            // Act
            var employees = repo.GetAllByDepartmentIdAndPosition(departmentId, positionId);

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && v.Position.Id == positionId && v.IsActive), employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByDepartmentIdAndPosition_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var positionId = _rnd.Next(1, TestData.positions.Count());

            // Act
            var employees = repo.GetAllByDepartmentIdAndPosition(999, positionId);

            // Assert
            Assert.Empty(employees);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByDepartmentIdAndPositions_ShouldReturnEmployees()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            var departmentId = _rnd.Next(1, TestData.departments.Count());

            // Act
            var employees = repo.GetAllByDepartmentIdAndPositions(departmentId, 1, 2);
            var employees2 = repo.GetAllByDepartmentIdAndPositions(departmentId, new int[] {1, 2});

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && v.Position.Id >= 1 && v.Position.Id <= 2 && v.IsActive), employees);
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && (v.Position.Id == 1 || v.Position.Id == 2) && v.IsActive), employees2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAllByDepartmentIdAndPositions_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            // Act
            var employees = repo.GetAllByDepartmentIdAndPositions(999, 1, 2);
            var employees2 = repo.GetAllByDepartmentIdAndPositions(999, new int[] {1, 2});

            // Assert
            Assert.Empty(employees);
            Assert.Empty(employees2);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnEmployee()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            int id = _rnd.Next(1, TestData.employees.Count());

            // Act
            var employee = repo.GetById(id);

            // Assert
            Assert.Equal(TestData.employees.SingleOrDefault(v => v.Id == id),
                employee);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            // Act
            var employee = repo.GetById(999);

            // Assert
            Assert.Null(employee);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddEmployee()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            int departmentId = _rnd.Next(1, TestData.departments.Count());
            int positionId = _rnd.Next(1, TestData.positions.Count());
            var employee = new Employee
            {
                Fullname = "NewCreate",
                Name = "NewCreate",
                Department = TestData.departments.SingleOrDefault(v => v.Id == departmentId),
                Position = TestData.positions.SingleOrDefault(v => v.Id == positionId),
                IsActive = true,
            };

            // Act
            repo.Add(employee);

            // Assert
            Assert.NotNull(repo.GetById(employee.Id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateEmployee()
        {
            // Arrange
            var employees = new List<Employee> { };
            foreach (var e in TestData.employees)
            {
                employees.Add(new Employee
                {
                    Id = e.Id,
                    Fullname = e.Fullname,
                    Name = e.Name,
                    Department = e.Department,
                    Position = e.Position,
                    IsActive = e.IsActive,
                });
            }
            var context = GetContext(employees);
            var repo = new SqlEmployeeRepo(context);

            int id = _rnd.Next(1, employees.Count());
            var employee = employees.FirstOrDefault(v => v.Id == id);
            employee.Name = "NewUpdate";

            // Act
            repo.Update(employee);

            // Assert
            Assert.Equal(employee.Name, repo.GetById(id).Name);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Delete_ShouldDeleteEmployee()
        {
            // Arrange
            var context = GetContext(TestData.employees);
            var repo = new SqlEmployeeRepo(context);

            int id = _rnd.Next(1, TestData.employees.Count());
            var employee = TestData.employees.FirstOrDefault(v => v.Id == id);

            // Act
            repo.Delete(employee);

            // Assert
            Assert.Null(repo.GetById(id));

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
