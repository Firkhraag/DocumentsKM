using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkApprovalRepoTest
    {
        private readonly Random _rnd = new Random();
        private readonly int _maxMarkId = 3;

        private ApplicationContext GetContext(List<MarkApproval> markApprovals)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "MarkApprovalTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Marks.AddRange(TestData.marks);
            context.Employees.AddRange(TestData.employees);
            context.MarkApprovals.AddRange(markApprovals);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnMarkApprovals()
        {
            // Arrange
            var context = GetContext(TestData.markApprovals);
            var repo = new SqlMarkApprovalRepo(context);

            var markId = _rnd.Next(1, _maxMarkId);

            // Act
            var markApprovals = repo.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.markApprovals.Where(v => v.Mark.Id == markId),
                markApprovals);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.markApprovals);
            var repo = new SqlMarkApprovalRepo(context);

            // Act
            var markApprovals = repo.GetAllByMarkId(999);

            // Assert
            Assert.Empty(markApprovals);

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Add_ShouldAddMarkApproval()
        {
            // Arrange
            var context = GetContext(TestData.markApprovals);
            var repo = new SqlMarkApprovalRepo(context);

            int markId = _rnd.Next(1, TestData.marks.Count());
            int employeeId = 3;
            var markApproval = new MarkApproval
            {
                Mark = TestData.marks.SingleOrDefault(v => v.Id == markId),
                MarkId = markId,
                Employee = TestData.employees.SingleOrDefault(v => v.Id == employeeId),
                EmployeeId = employeeId,
            };

            // Act
            repo.Add(markApproval);

            // Assert
            Assert.Equal(
                TestData.markApprovals.Where(v => v.Mark.Id == markId).Count() + 1,
                repo.GetAllByMarkId(markId).Count());

            context.Database.EnsureDeleted();
        }

        [Fact]
        public void Delete_ShouldDeleteMarkApproval()
        {
            // Arrange
            var context = GetContext(TestData.markApprovals);
            var repo = new SqlMarkApprovalRepo(context);

            var markApproval = TestData.markApprovals[0];

            // Act
            repo.Delete(markApproval);

            // Assert
            Assert.Equal(TestData.markApprovals.Where(
                v => v.MarkId == markApproval.MarkId).Count() - 1,
                repo.GetAllByMarkId(markApproval.MarkId).Count());

            context.Database.EnsureDeleted();
        }
    }
}
