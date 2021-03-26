using System;
using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class MarkApprovalServiceTest
    {
        private readonly Mock<IMarkApprovalRepo> _repository = new Mock<IMarkApprovalRepo>();
        private readonly IMarkApprovalService _service;
        private readonly Random _rnd = new Random();
        private readonly List<MarkApproval> _markApprovals = new List<MarkApproval> { };
        private readonly int _maxMarkId = 3;

        public MarkApprovalServiceTest()
        {
            var mockMarkRepo = new Mock<IMarkRepo>();
            var mockEmployeeRepo = new Mock<IEmployeeRepo>();

            // Arrange
            foreach (var markApproval in TestData.markApprovals)
            {
                _markApprovals.Add(new MarkApproval
                {
                    Mark = markApproval.Mark,
                    Employee = markApproval.Employee,
                });
            }
            foreach (var mark in TestData.marks)
            {
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _markApprovals.Where(v => v.Mark.Id == mark.Id));
            }
            foreach (var employee in TestData.employees)
            {
                mockEmployeeRepo.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<MarkApproval>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<MarkApproval>())).Verifiable();

            _service = new MarkApprovalService(
                _repository.Object,
                mockMarkRepo.Object,
                mockEmployeeRepo.Object);
        }

        [Fact]
        public void GetAllEmployeesByMarkId_ShouldReturnEmployees()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var employees = _service.GetAllEmployeesByMarkId(markId);

            // Assert
            Assert.Equal(_markApprovals.Where(
                v => v.Mark.Id == markId).Select(v => v.Employee), employees);
        }

        [Fact]
        public void GetAllEmployeesByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Act
            var employees = _service.GetAllEmployeesByMarkId(999);

            // Assert
            Assert.Empty(employees);
        }

        [Fact]
        public void Update_ShouldUpdateMarkApproval()
        {
            // Arrange
            int markId = 1;
            List<int> employeeIds = new List<int> { 2, 3 };

            // Act
            _service.Update(markId, employeeIds);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<MarkApproval>()), Times.Once);
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkApproval>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            List<int> employeeIds = new List<int> { _rnd.Next(1, TestData.employees.Count()) };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(999, employeeIds));
            Assert.Throws<ArgumentNullException>(() => _service.Update(markId, new List<int> { 999 }));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkApproval>()), Times.Never);
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkApproval>()), Times.Never);
        }
    }
}
