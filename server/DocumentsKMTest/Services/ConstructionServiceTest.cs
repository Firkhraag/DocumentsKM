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
    public class ConstructionServiceTest
    {
        private readonly Mock<IConstructionRepo> _repository = new Mock<IConstructionRepo>();
        private readonly Mock<ISpecificationRepo> _mockSpecificationRepo = new Mock<ISpecificationRepo>();
        private readonly Mock<IConstructionTypeRepo> _mockConstructionTypeRepo = new Mock<IConstructionTypeRepo>();
        private readonly Mock<IConstructionSubtypeRepo> _mockConstructionSubtypeRepo = new Mock<IConstructionSubtypeRepo>();
        private readonly Mock<IWeldingControlRepo> _mockWeldingControlRepo = new Mock<IWeldingControlRepo>();
        private readonly IConstructionService _service;
        private readonly Random _rnd = new Random();
        private readonly List<Construction> _constructions = new List<Construction> { };
        private readonly int _maxSpecificationId = 3;

        public ConstructionServiceTest()
        {
            // Arrange
            foreach (var c in TestData.constructions)
            {
                _constructions.Add(new Construction
                {
                    Id = c.Id,
                    Specification = c.Specification,
                    Name = c.Name,
                    Type = c.Type,
                    Subtype = c.Subtype,
                    Valuation = c.Valuation,
                    NumOfStandardConstructions = c.NumOfStandardConstructions,
                    HasEdgeBlunting = c.HasEdgeBlunting,
                    HasDynamicLoad = c.HasDynamicLoad,
                    HasFlangedConnections = c.HasFlangedConnections,
                    WeldingControl = c.WeldingControl,
                    PaintworkCoeff = c.PaintworkCoeff,
                });
            }
            foreach (var construction in _constructions)
            {
                _repository.Setup(mock =>
                    mock.GetById(construction.Id)).Returns(
                        _constructions.SingleOrDefault(v => v.Id == construction.Id));
            }
            foreach (var specification in TestData.specifications)
            {
                _mockSpecificationRepo.Setup(mock =>
                    mock.GetById(specification.Id)).Returns(
                        TestData.specifications.SingleOrDefault(v => v.Id == specification.Id));

                _repository.Setup(mock =>
                    mock.GetAllBySpecificationId(specification.Id)).Returns(
                        _constructions.Where(v => v.Specification.Id == specification.Id));

                foreach (var construction in _constructions)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(
                            specification.Id, construction.Name, construction.PaintworkCoeff)).Returns(
                                _constructions.SingleOrDefault(
                                    v => v.Specification.Id == specification.Id &&
                                        v.Name == construction.Name &&
                                            v.PaintworkCoeff == construction.PaintworkCoeff));
                }
            }
            foreach (var type in TestData.constructionTypes)
            {
                _mockConstructionTypeRepo.Setup(mock =>
                    mock.GetById(type.Id)).Returns(
                        TestData.constructionTypes.SingleOrDefault(v => v.Id == type.Id));
            }
            foreach (var subtype in TestData.constructionSubtypes)
            {
                _mockConstructionSubtypeRepo.Setup(mock =>
                    mock.GetById(subtype.Id)).Returns(
                        TestData.constructionSubtypes.SingleOrDefault(v => v.Id == subtype.Id));
            }
            foreach (var weldingControl in TestData.weldingControl)
            {
                _mockWeldingControlRepo.Setup(mock =>
                    mock.GetById(weldingControl.Id)).Returns(
                        TestData.weldingControl.SingleOrDefault(v => v.Id == weldingControl.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<Construction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<Construction>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<Construction>())).Verifiable();

            _service = new ConstructionService(
                _repository.Object,
                _mockSpecificationRepo.Object,
                _mockConstructionTypeRepo.Object,
                _mockConstructionSubtypeRepo.Object,
                _mockWeldingControlRepo.Object);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnConstructions()
        {
            // Arrange
            int specificationId = _rnd.Next(1, _maxSpecificationId);

            // Act
            var returnedConstructions = _service.GetAllBySpecificationId(
                specificationId);

            // Assert
            Assert.Equal(_constructions.Where(
                v => v.Specification.Id == specificationId), returnedConstructions);
        }

        [Fact]
        public void GetAllBySpecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Act
            var returnedConstructions = _service.GetAllBySpecificationId(999);

            // Assert
            Assert.Empty(returnedConstructions);
        }

        [Fact]
        public void Create_ShouldCreateConstruction()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            int subtypeId = _rnd.Next(1, TestData.constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());

            var newConstruction = new Construction
            {
                Name = "NewCreate",
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = 2,
            };

            // Act
            _service.Create(
                newConstruction,
                specificationId,
                typeId,
                subtypeId,
                weldingControlId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Once);
            Assert.NotNull(newConstruction.Specification);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            int subtypeId = _rnd.Next(1, TestData.constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());

            var newConstruction = new Construction
            {
                Name = "NewCreate",
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = 2,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    null, specificationId, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, 999, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, 999, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, 999, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, subtypeId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValue()
        {
            // Arrange
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            int subtypeId = _rnd.Next(1, TestData.constructionSubtypes.Count());
            int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());

            var conflictSpecificationId = _constructions[0].Specification.Id;
            var conflictName = _constructions[0].Name;
            var conflictPaintworkCoeff = _constructions[0].PaintworkCoeff;

            var newConstruction = new Construction
            {
                Name = conflictName,
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                PaintworkCoeff = conflictPaintworkCoeff,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(
                () => _service.Create(
                    newConstruction,
                    conflictSpecificationId,
                    typeId,
                    subtypeId,
                    weldingControlId));

            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());
            var newStringValue = "NewUpdate";
            var newBoolValue = true;
            var newIntValue = 99;

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Name = newStringValue,
                Valuation = newStringValue,
                NumOfStandardConstructions = newIntValue,
                HasEdgeBlunting = newBoolValue,
                HasDynamicLoad = newBoolValue,
                HasFlangedConnections = newBoolValue,
                PaintworkCoeff = newIntValue,
            };

            // Act
            _service.Update(id, newConstructionRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Once);
            Assert.Equal(
                newStringValue, _constructions.SingleOrDefault(v => v.Id == id).Name);
            Assert.Equal(
                newStringValue, _constructions.SingleOrDefault(v => v.Id == id).Valuation);
            Assert.Equal(
                newIntValue, _constructions.SingleOrDefault(v => v.Id == id).NumOfStandardConstructions);
            Assert.Equal(
                newBoolValue, _constructions.SingleOrDefault(v => v.Id == id).HasEdgeBlunting);
            Assert.Equal(
                newBoolValue, _constructions.SingleOrDefault(v => v.Id == id).HasDynamicLoad);
            Assert.Equal(
                newBoolValue, _constructions.SingleOrDefault(v => v.Id == id).HasFlangedConnections);
            Assert.Equal(
                newIntValue, _constructions.SingleOrDefault(v => v.Id == id).PaintworkCoeff);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Valuation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, newConstructionRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            var id = _constructions[1].Id;
            var conflictName = _constructions[0].Name;
            var conflictPaintworkCoeff = _constructions[0].PaintworkCoeff;

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Name = conflictName,
                PaintworkCoeff = conflictPaintworkCoeff,
                Valuation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id, newConstructionRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<Construction>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<Construction>()), Times.Never);
        }
    }
}
