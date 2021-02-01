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
    public class AdditionalWorkServiceTest
    {
        private readonly Mock<IAdditionalWorkRepo> _repository = new Mock<IAdditionalWorkRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly Mock<IDocRepo> _mockDocRepo = new Mock<IDocRepo>();
        private readonly IAdditionalWorkService _service;
        private readonly Random _rnd = new Random();
        private readonly List<AdditionalWork> _additionalWork = new List<AdditionalWork> { };
        private readonly int _maxMarkId = 3;

        public AdditionalWorkServiceTest()
        {
            // Arrange
            foreach (var additionalWork in TestData.additionalWork)
            {
                _additionalWork.Add(new AdditionalWork
                {
                    Id = additionalWork.Id,
                    Mark = additionalWork.Mark,
                    Employee = additionalWork.Employee,
                    Valuation = additionalWork.Valuation,
                    MetalOrder = additionalWork.MetalOrder,
                });
            }
            foreach (var additionalWork in _additionalWork)
            {
                _repository.Setup(mock =>
                    mock.GetById(additionalWork.Id)).Returns(
                        _additionalWork.SingleOrDefault(v => v.Id == additionalWork.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _additionalWork.Where(v => v.Mark.Id == mark.Id));
            }
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));

                foreach (var mark in TestData.marks)
                {
                    foreach (var additionalWork in _additionalWork)
                    {
                        _repository.Setup(mock =>
                            mock.GetByUniqueKey(mark.Id, employee.Id)).Returns(
                                _additionalWork.SingleOrDefault(
                                    v => v.Mark.Id == mark.Id && v.Employee.Id == employee.Id));
                    }
                }
            }
            foreach (var doc in TestData.docs)
            {
                _mockDocRepo.Setup(mock =>
                    mock.GetById(doc.Id)).Returns(
                        TestData.docs.SingleOrDefault(v => v.Id == doc.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<AdditionalWork>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<AdditionalWork>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<AdditionalWork>())).Verifiable();

            _service = new AdditionalWorkService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockEmployeeRepo.Object,
                _mockDocRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAdditionalWork()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var returnedAdditionalWork = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(_additionalWork.Where(
                v => v.Mark.Id == markId).Count(), returnedAdditionalWork.Count());
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Act
            var returnedAdditionalWorks = _service.GetAllByMarkId(999);

            // Assert
            Assert.Empty(returnedAdditionalWorks);
        }

        [Fact]
        public void Create_ShouldCreateAdditionalWork()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int employeeId = _rnd.Next(1, TestData.employees.Count());
            while (_additionalWork.SingleOrDefault(
                v => v.Mark.Id == markId && v.Employee.Id == employeeId) != null)
            {
                markId = _rnd.Next(1, TestData.marks.Count());
                employeeId = _rnd.Next(1, TestData.employees.Count());
            }

            var newAdditionalWork = new AdditionalWork
            {
                Valuation = 9,
                MetalOrder = 9,
            };

            // Act
            _service.Create(newAdditionalWork, markId, employeeId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<AdditionalWork>()), Times.Once);
            Assert.NotNull(newAdditionalWork.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int employeeId = _rnd.Next(1, TestData.employees.Count());

            var newAdditionalWork = new AdditionalWork
            {
                Valuation = 9,
                MetalOrder = 9,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null, markId, employeeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newAdditionalWork, 999, employeeId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newAdditionalWork, markId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<AdditionalWork>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            var conflictMarkId = _additionalWork[0].Mark.Id;
            var conflictEmployeeId = _additionalWork[0].Employee.Id;

            var newAdditionalWork = new AdditionalWork
            {
                Valuation = 10,
                MetalOrder = 10,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(
                newAdditionalWork, conflictMarkId, conflictEmployeeId));

            _repository.Verify(mock => mock.Add(It.IsAny<AdditionalWork>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateAdditionalWork()
        {
            // Arrange
            int id = _rnd.Next(1, _additionalWork.Count());
            var newIntValue = 99;

            var newAdditionalWorkRequest = new AdditionalWorkUpdateRequest
            {
                Valuation = newIntValue,
                MetalOrder = newIntValue,
            };

            // Act
            _service.Update(id, newAdditionalWorkRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<AdditionalWork>()), Times.Once);
            var v = _additionalWork.SingleOrDefault(v => v.Id == id);
            Assert.Equal(newIntValue, v.Valuation);
            Assert.Equal(newIntValue, v.MetalOrder);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _additionalWork.Count());

            var newAdditionalWorkRequest = new AdditionalWorkUpdateRequest
            {
                Valuation = 11,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(999, newAdditionalWorkRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<AdditionalWork>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            var conflictEmployeeId = _additionalWork[0].Employee.Id;
            var id = _additionalWork[1].Id;

            var newAdditionalWorkRequest = new AdditionalWorkUpdateRequest
            {
                EmployeeId = conflictEmployeeId,
                Valuation = 11,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id, newAdditionalWorkRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<AdditionalWork>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteAdditionalWork()
        {
            // Arrange
            int id = _rnd.Next(1, _additionalWork.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<AdditionalWork>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(It.IsAny<AdditionalWork>()), Times.Never);
        }
    }
}
