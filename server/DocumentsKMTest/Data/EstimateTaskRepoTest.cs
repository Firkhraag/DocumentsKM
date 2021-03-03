using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EstimateTaskRepoTest
    {
        private readonly Random _rnd = new Random();

        private ApplicationContext GetContext(List<EstimateTask> estimateTask)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase(databaseName: "EstimateTaskTestDb");
            var options = builder.Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Employees.AddRange(TestData.employees);
            context.EstimateTask.AddRange(estimateTask);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetByMarkId_ShouldReturnEstimateTask()
        {
            // Arrange
            var context = GetContext(TestData.estimateTask);
            var repo = new SqlEstimateTaskRepo(context);

            var markId = _rnd.Next(1, TestData.marks.Count());

            // Act
            var estimateTask = repo.GetByMarkId(markId);

            // Assert
            Assert.Equal(
                TestData.estimateTask.FirstOrDefault(v => v.Mark.Id == markId),
                estimateTask);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetByMarkId_ShouldReturnNull_WhenWrongMarkId()
        {
            // Arrange
            var context = GetContext(TestData.estimateTask);
            var repo = new SqlEstimateTaskRepo(context);

            // Act
            var estimateTask = repo.GetByMarkId(999);

            // Assert
            Assert.Null(estimateTask);

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Add_ShouldAddEstimateTask()
        {
            // Arrange
            var context = GetContext(TestData.estimateTask);
            var repo = new SqlEstimateTaskRepo(context);

            int markId = 4;
            int employeeId = _rnd.Next(1, TestData.employees.Count());
            var estimateTask = new EstimateTask
            {
                TaskText = "NewCreate",
                AdditionalText = "NewCreate",
                Mark = TestData.marks.FirstOrDefault(v => v.Id == markId),
            };

            // Act
            repo.Add(estimateTask);

            // Assert
            Assert.NotNull(repo.GetByMarkId(markId));

            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void Update_ShouldUpdateEstimateTask()
        {
            // Arrange
            var estimateTaskList = new List<EstimateTask> { };
            foreach (var et in TestData.estimateTask)
            {
                estimateTaskList.Add(new EstimateTask
                {
                    Mark = et.Mark,
                    TaskText = et.TaskText,
                    AdditionalText = et.AdditionalText,
                    ApprovalEmployee = et.ApprovalEmployee,
                });
            }
            var context = GetContext(estimateTaskList);
            var repo = new SqlEstimateTaskRepo(context);

            int markId = 1;
            var estimateTask = estimateTaskList.FirstOrDefault(v => v.Mark.Id == markId);
            estimateTask.AdditionalText = "NewUpdate";

            // Act
            repo.Update(estimateTask);

            // Assert
            Assert.Equal(estimateTask.AdditionalText, repo.GetByMarkId(markId).AdditionalText);

            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
