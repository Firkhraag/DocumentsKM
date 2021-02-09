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
    public class ConstructionBoltServiceTest
    {
        private readonly Mock<IConstructionBoltRepo> _repository = new Mock<IConstructionBoltRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IConstructionRepo> _mockConstructionRepo = new Mock<IConstructionRepo>();
        private readonly Mock<IBoltDiameterRepo> _mockBoltDiameterRepo = new Mock<IBoltDiameterRepo>();
        private readonly IConstructionBoltService _service;
        private readonly Random _rnd = new Random();
        private readonly List<ConstructionBolt> _constructionBolts = new List<ConstructionBolt> { };
        private readonly int _maxConstructionId = 3;

        public ConstructionBoltServiceTest()
        {
            // Arrange
            foreach (var cb in TestData.constructionBolts)
            {
                _constructionBolts.Add(new ConstructionBolt
                {
                    Id = cb.Id,
                    Construction = cb.Construction,
                    Diameter = cb.Diameter,
                    Packet = cb.Packet,
                    Num = cb.Num,
                    NutNum = cb.NutNum,
                    WasherNum = cb.WasherNum,
                });
            }
            foreach (var constructionBolt in _constructionBolts)
            {
                _repository.Setup(mock =>
                    mock.GetById(constructionBolt.Id)).Returns(
                        _constructionBolts.SingleOrDefault(v => v.Id == constructionBolt.Id));
            }
            foreach (var construction in TestData.constructions)
            {
                _mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, false)).Returns(
                        TestData.constructions.SingleOrDefault(v => v.Id == construction.Id));
                _mockConstructionRepo.Setup(mock =>
                    mock.GetById(construction.Id, true)).Returns(
                        TestData.constructions.SingleOrDefault(v => v.Id == construction.Id));

                _repository.Setup(mock =>
                    mock.GetAllByConstructionId(construction.Id)).Returns(
                        _constructionBolts.Where(v => v.Construction.Id == construction.Id));

                // foreach (var ConstructionBolt in _constructionBolts)
                // {
                //     _repository.Setup(mock =>
                //         mock.GetByUniqueKey(mark.Id, ConstructionBolt.Designation)).Returns(
                //             _constructionBolts.SingleOrDefault(
                //                 v => v.Mark.Id == mark.Id && v.Designation == ConstructionBolt.Designation));
                // }
            }
            foreach (var diameter in TestData.boltDiameters)
            {
                _mockBoltDiameterRepo.Setup(mock =>
                    mock.GetById(diameter.Id)).Returns(
                        TestData.boltDiameters.SingleOrDefault(
                            v => v.Id == diameter.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<ConstructionBolt>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<ConstructionBolt>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<ConstructionBolt>())).Verifiable();

            _service = new ConstructionBoltService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockConstructionRepo.Object,
                _mockBoltDiameterRepo.Object);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructionBolts()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var returnedConstructionBolts = _service.GetAllByConstructionId(
                constructionId);

            // Assert
            Assert.Equal(_constructionBolts.Where(
                v => v.Construction.Id == constructionId), returnedConstructionBolts);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Act
            var returnedConstructionBolts = _service.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(returnedConstructionBolts);
        }

        [Fact]
        public void Create_ShouldCreateConstructionBolt()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.marks.Count());
            int diameterId = _rnd.Next(1, TestData.boltDiameters.Count());

            var newConstructionBolt = new ConstructionBolt
            {
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act
            _service.Create(newConstructionBolt, constructionId, diameterId);

            // Assert
            _repository.Verify(mock => mock.Add(
                It.IsAny<ConstructionBolt>()), Times.Once);
            Assert.NotNull(newConstructionBolt.Construction);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.marks.Count());
            int diameterId = _rnd.Next(1, TestData.boltDiameters.Count());

            var newConstructionBolt = new ConstructionBolt
            {
                Packet = 5,
                Num = 5,
                NutNum = 5,
                WasherNum = 5,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null, constructionId, diameterId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionBolt, 999, diameterId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionBolt, constructionId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<ConstructionBolt>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictConstructionId = _constructionBolts[0].Mark.Id;
        //     var conflictDesignation = _constructionBolts[0].Designation;

        //     var newConstructionBolt = new ConstructionBolt
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewCreate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Create(newConstructionBolt, conflictConstructionId));

        //     _repository.Verify(mock => mock.Add(It.IsAny<ConstructionBolt>()), Times.Never);
        // }

        [Fact]
        public void Update_ShouldUpdateConstructionBolt()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionBolts.Count());
            var newNumber = 6;

            var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
            {
                Packet = newNumber,
                Num = newNumber,
                NutNum = newNumber,
                WasherNum = newNumber,
            };

            // Act
            _service.Update(id, newConstructionBoltRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<ConstructionBolt>()), Times.Once);
            Assert.Equal(newNumber, _constructionBolts.SingleOrDefault(
                v => v.Id == id).Packet);
            Assert.Equal(newNumber, _constructionBolts.SingleOrDefault(
                v => v.Id == id).Num);
            Assert.Equal(newNumber, _constructionBolts.SingleOrDefault(
                v => v.Id == id).NutNum);
            Assert.Equal(newNumber, _constructionBolts.SingleOrDefault(
                v => v.Id == id).WasherNum);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionBolts.Count());
            int wrongId = 999;
            var newNumber = 6;

            var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
            {
                Packet = newNumber,
                Num = newNumber,
                NutNum = newNumber,
                WasherNum = newNumber,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                wrongId, newConstructionBoltRequest));

            _repository.Verify(mock => mock.Update(
                It.IsAny<ConstructionBolt>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictDesignation = _constructionBolts[0].Designation;
        //     var id = _constructionBolts[3].Id;

        //     var newConstructionBoltRequest = new ConstructionBoltUpdateRequest
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewUpdate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newConstructionBoltRequest));

        //     _repository.Verify(mock => mock.Update(
        //         It.IsAny<ConstructionBolt>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteConstructionBolt()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionBolts.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionBolt>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionBolt>()), Times.Never);
        }
    }
}
