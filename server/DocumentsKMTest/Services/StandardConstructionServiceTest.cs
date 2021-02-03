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
    public class StandardConstructionServiceTest
    {
        private readonly Mock<IStandardConstructionRepo> _repository = new Mock<IStandardConstructionRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
        private readonly IStandardConstructionService _service;
        private readonly Random _rnd = new Random();
        private readonly List<StandardConstruction> _standardConstructions = new List<StandardConstruction> { };
        private readonly int _maxSpecificationId = 3;

        public StandardConstructionServiceTest()
        {
            // Arrange
            foreach (var sc in TestData.standardConstructions)
            {
                _standardConstructions.Add(new StandardConstruction
                {
                    Id = sc.Id,
                    Specification = sc.Specification,
                    Name = sc.Name,
                    Num = sc.Num,
                    Sheet = sc.Sheet,
                    Weight = sc.Weight,
                });
            }
            foreach (var standardConstruction in _standardConstructions)
            {
                _repository.Setup(mock =>
                    mock.GetById(standardConstruction.Id)).Returns(
                        _standardConstructions.SingleOrDefault(v => v.Id == standardConstruction.Id));
            }
            foreach (var specification in TestData.specifications)
            {
                _mockSpecificationRepo.Setup(mock =>
                    mock.GetById(specification.Id)).Returns(
                        TestData.specifications.SingleOrDefault(v => v.Id == specification.Id));

                _repository.Setup(mock =>
                    mock.GetAllBySpecificationId(specification.Id)).Returns(
                        _standardConstructions.Where(v => v.Specification.Id == specification.Id));

                // foreach (var standardConstruction in _standardConstructions)
                // {
                //     _repository.Setup(mock =>
                //         mock.GetByUniqueKey(
                //             specification.Id, standardConstruction.Name, standardConstruction.PaintworkCoeff)).Returns(
                //                 _standardConstructions.SingleOrDefault(
                //                     v => v.Specification.Id == specification.Id &&
                //                         v.Name == standardConstruction.Name &&
                //                             v.PaintworkCoeff == standardConstruction.PaintworkCoeff));
                // }
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<StandardConstruction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<StandardConstruction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<StandardConstruction>())).Verifiable();

            _service = new StandardConstructionService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockSpecificationRepo.Object);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnStandardConstructions()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var returnedStandardConstructions = _service.GetAllBySpecificationId(
                specificationId);

            // Assert
            Assert.Equal(_standardConstructions.Where(
                v => v.Specification.Id == specificationId), returnedStandardConstructions);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Act
            var returnedstandardConstructions = _service.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(returnedstandardConstructions);
        }

        [Fact]
        public void Create_ShouldCreatestandardConstruction()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());

            var newStandardConstruction = new StandardConstruction
            {
                Name = "NewCreate",
                Num = 9,
                Sheet = "NewCreate",
                Weight = 9.0f,
            };

            // Act
            _service.Create(
                newStandardConstruction,
                specificationId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<StandardConstruction>()), Times.Once);
            Assert.NotNull(newStandardConstruction.Specification);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());

            var newStandardConstruction = new StandardConstruction
            {
                Name = "NewCreate",
                Num = 9,
                Sheet = "NewCreate",
                Weight = 9.0f,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    null, specificationId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newStandardConstruction, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<StandardConstruction>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict_WhenConflictValue()
        // {
        //     // Arrange
        //     int typeId = _rnd.Next(1, TestData.standardConstructionTypes.Count());
        //     int subtypeId = _rnd.Next(1, TestData.standardConstructionSubtypes.Count());
        //     int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());

        //     var conflictSpecificationId = _standardConstructions[0].Specification.Id;
        //     var conflictName = _standardConstructions[0].Name;
        //     var conflictPaintworkCoeff = _standardConstructions[0].PaintworkCoeff;

        //     var newstandardConstruction = new standardConstruction
        //     {
        //         Name = conflictName,
        //         Valuation = "NewCreate",
        //         NumOfStandardstandardConstructions = 0,
        //         HasEdgeBlunting = true,
        //         HasDynamicLoad = false,
        //         HasFlangedConnections = true,
        //         PaintworkCoeff = conflictPaintworkCoeff,
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(
        //         () => _service.Create(
        //             newstandardConstruction,
        //             conflictSpecificationId,
        //             typeId,
        //             subtypeId,
        //             weldingControlId));

        //     _repository.Verify(mock => mock.Add(It.IsAny<standardConstruction>()), Times.Never);
        // }

        [Fact]
        public void Update_ShouldUpdateStandardConstruction()
        {
            // Arrange
            var id = _rnd.Next(1, _standardConstructions.Count());
            var newStringValue = "NewUpdate";
            var newIntValue = 99;
            var newFloatValue = 9.0f;

            var newStandardConstructionRequest = new StandardConstructionUpdateRequest
            {
                Name = newStringValue,
                Num = newIntValue,
                Sheet = newStringValue,
                Weight = newFloatValue,
            };

            // Act
            _service.Update(id, newStandardConstructionRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<StandardConstruction>()), Times.Once);
            Assert.Equal(
                newStringValue, _standardConstructions.SingleOrDefault(v => v.Id == id).Name);
            Assert.Equal(
                newIntValue, _standardConstructions.SingleOrDefault(v => v.Id == id).Num);
            Assert.Equal(
                newStringValue, _standardConstructions.SingleOrDefault(v => v.Id == id).Sheet);
            Assert.Equal(
                newFloatValue, _standardConstructions.SingleOrDefault(v => v.Id == id).Weight);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _standardConstructions.Count());

            var newStandardConstructionRequest = new StandardConstructionUpdateRequest
            {
                Sheet = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, newStandardConstructionRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<StandardConstruction>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var id = _standardConstructions[1].Id;
        //     var conflictName = _standardConstructions[0].Name;
        //     var conflictPaintworkCoeff = _standardConstructions[0].PaintworkCoeff;

        //     var newstandardConstructionRequest = new standardConstructionUpdateRequest
        //     {
        //         Name = conflictName,
        //         PaintworkCoeff = conflictPaintworkCoeff,
        //         Valuation = "NewUpdate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newstandardConstructionRequest));

        //     _repository.Verify(mock => mock.Update(It.IsAny<standardConstruction>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteStandardConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _standardConstructions.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<StandardConstruction>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<StandardConstruction>()), Times.Never);
        }
    }
}
