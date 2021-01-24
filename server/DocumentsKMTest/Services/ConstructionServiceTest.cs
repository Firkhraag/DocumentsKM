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
        public void GetAllByspecificationId_ShouldReturnConstructions()
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
        public void GetAllByspecificationId_ShouldReturnEmptyArray_WhenWrongSpecificationId()
        {
            // Arrange
            int wrongSpecificationId = 999;

            // Act
            var returnedConstructions = _service.GetAllBySpecificationId(
                wrongSpecificationId);

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
                Specification = TestData.specifications.SingleOrDefault(
                    v => v.Id == specificationId),
                Name = "NewCreate",
                Type = TestData.constructionTypes.SingleOrDefault(
                    v => v.Id == typeId),
                Subtype = TestData.constructionSubtypes.SingleOrDefault(
                    v => v.Id == subtypeId),
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = TestData.weldingControl.SingleOrDefault(
                    v => v.Id == weldingControlId),
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
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int specificationId = _rnd.Next(1, TestData.specifications.Count());
            int wrongSpecificationId = 999;
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());
            int wrongTypeId = 999;
            int subtypeId = _rnd.Next(1, TestData.constructionSubtypes.Count());
            int wrongSubtypeId = 999;
            int weldingControlId = _rnd.Next(1, TestData.weldingControl.Count());
            int wrongWeldingControlId = 999;

            var newConstruction = new Construction
            {
                Specification = TestData.specifications.SingleOrDefault(
                    v => v.Id == specificationId),
                Name = "NewCreate",
                Type = TestData.constructionTypes.SingleOrDefault(
                    v => v.Id == typeId),
                Subtype = TestData.constructionSubtypes.SingleOrDefault(
                    v => v.Id == subtypeId),
                Valuation = "NewCreate",
                NumOfStandardConstructions = 0,
                HasEdgeBlunting = true,
                HasDynamicLoad = false,
                HasFlangedConnections = true,
                WeldingControl = TestData.weldingControl.SingleOrDefault(
                    v => v.Id == weldingControlId),
                PaintworkCoeff = 2,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    null, specificationId, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, wrongSpecificationId, typeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, wrongTypeId, subtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, wrongSubtypeId, weldingControlId));
            Assert.Throws<ArgumentNullException>(
                () => _service.Create(
                    newConstruction, specificationId, typeId, subtypeId, wrongWeldingControlId));

            _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictspecificationId = _constructions[0].Mark.Id;
        //     var conflictDesignation = _constructions[0].Designation;

        //     var newConstruction = new Construction
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewCreate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(
        //         () => _service.Create(newConstruction, conflictspecificationId));

        //     _repository.Verify(mock => mock.Add(It.IsAny<Construction>()), Times.Never);
        // }

        [Fact]
        public void Update_ShouldUpdateConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());
            var newDesignation = "NewUpdate";

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Valuation = "NewUpdate",
            };

            // Act
            _service.Update(id, newConstructionRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Once);
            Assert.Equal(
                newDesignation, _constructions.SingleOrDefault(v => v.Id == id).Valuation);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());
            int wrongId = 999;

            var newConstructionRequest = new ConstructionUpdateRequest
            {
                Valuation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                wrongId, newConstructionRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictDesignation = _Constructions[0].Designation;
        //     var id = _Constructions[3].Id;

        //     var newConstructionRequest = new ConstructionUpdateRequest
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewUpdate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newConstructionRequest));

        //     _repository.Verify(mock => mock.Update(It.IsAny<Construction>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteConstruction()
        {
            // Arrange
            int id = _rnd.Next(1, _constructions.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<Construction>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            var wrongId = 999;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(wrongId));

            _repository.Verify(mock => mock.Delete(It.IsAny<Construction>()), Times.Never);
        }
    }
}
