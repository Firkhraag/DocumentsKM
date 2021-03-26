using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EstimateTaskServiceTest
    {
        private readonly Mock<IEstimateTaskRepo> _repository = new Mock<IEstimateTaskRepo>();
        private readonly IEstimateTaskService _service;
        private readonly Random _rnd = new Random();
        private readonly List<EstimateTask> _estimateTask =
            new List<EstimateTask> { };
        private readonly int _maxMarkId = 2;

        public EstimateTaskServiceTest()
        {
            var mockMarkRepo = new Mock<IMarkRepo>();
            var mockEmployeeRepo = new Mock<IEmployeeRepo>();

            // Arrange
            foreach (var et in TestData.estimateTask)
            {
                _estimateTask.Add(new EstimateTask
                {
                    Mark = et.Mark,
                    TaskText = et.TaskText,
                    AdditionalText = et.AdditionalText,
                    ApprovalEmployee = et.ApprovalEmployee,
                });
            }

            foreach (var mark in TestData.marks)
            {
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetByMarkId(mark.Id)).Returns(
                        _estimateTask.SingleOrDefault(v => v.Mark.Id == mark.Id));
            }
            foreach (var employee in TestData.employees)
            {
                mockEmployeeRepo.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<EstimateTask>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<EstimateTask>())).Verifiable();

            _service = new EstimateTaskService(
                _repository.Object,
                mockMarkRepo.Object,
                mockEmployeeRepo.Object);
        }

        [Fact]
        public void GetByMarkId_ShouldReturnEstimateTask()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var estimateTask = _service.GetByMarkId(markId);

            // Assert
            Assert.NotNull(estimateTask);
        }

        [Fact]
        public void Update_ShouldUpdateEstimateTask()
        {
            // Arrange
            int markId = 2;
            int employeeId = 1;

            var estimateTaskRequest = new EstimateTaskUpdateRequest
            {
                TaskText = "NewUpdate",
                AdditionalText = "NewUpdate",
                ApprovalEmployeeId = employeeId,
            };

            // Act
            _service.Update(markId,
                estimateTaskRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<EstimateTask>()), Times.Once);
            Assert.Equal("NewUpdate", _estimateTask.SingleOrDefault(
                v => v.Mark.Id == markId).TaskText);
            Assert.Equal("NewUpdate", _estimateTask.SingleOrDefault(
                v => v.Mark.Id == markId).AdditionalText);
            Assert.Equal(employeeId, _estimateTask.SingleOrDefault(
                v => v.Mark.Id == markId).ApprovalEmployee.Id);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = 1;

            var estimateTaskRequest = new EstimateTaskUpdateRequest
            {
                AdditionalText = "NewUpdate",
            };
            var wrongEstimateTaskRequest1 = new EstimateTaskUpdateRequest
            {
                ApprovalEmployeeId = 999,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999,
                estimateTaskRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                markId,
                wrongEstimateTaskRequest1));

            _repository.Verify(mock => mock.Update(It.IsAny<EstimateTask>()), Times.Never);
        }
    }
}
