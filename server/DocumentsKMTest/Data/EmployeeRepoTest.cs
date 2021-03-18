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

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && v.Position.Id >= 1 && v.Position.Id <= 2 && v.IsActive), employees);

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

            // Assert
            Assert.Empty(employees);

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
    }
}
