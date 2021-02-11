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
    public class ConstructionElementServiceTest
    {
        private readonly Mock<IConstructionElementRepo> _repository = new Mock<IConstructionElementRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IConstructionRepo> _mockConstructionRepo = new Mock<IConstructionRepo>();
        private readonly Mock<IProfileRepo> _mockProfileRepo = new Mock<IProfileRepo>();
        private readonly Mock<ISteelRepo> _mockSteelRepo = new Mock<ISteelRepo>();
        private readonly IConstructionElementService _service;
        private readonly Random _rnd = new Random();
        private readonly List<ConstructionElement> _constructionElements = new List<ConstructionElement> { };
        private readonly int _maxConstructionId = 3;

        public ConstructionElementServiceTest()
        {
            // Arrange
            foreach (var ce in TestData.constructionElements)
            {
                _constructionElements.Add(new ConstructionElement
                {
                    Id = ce.Id,
                    Construction = ce.Construction,
                    Profile = ce.Profile,
                    Steel = ce.Steel,
                    Length = ce.Length,
                });
            }
            foreach (var constructionElement in _constructionElements)
            {
                _repository.Setup(mock =>
                    mock.GetById(constructionElement.Id)).Returns(
                        _constructionElements.SingleOrDefault(v => v.Id == constructionElement.Id));
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
                        _constructionElements.Where(v => v.Construction.Id == construction.Id));

                // foreach (var ConstructionElement in _constructionElements)
                // {
                //     _repository.Setup(mock =>
                //         mock.GetByUniqueKey(mark.Id, ConstructionElement.Designation)).Returns(
                //             _constructionElements.SingleOrDefault(
                //                 v => v.Mark.Id == mark.Id && v.Designation == ConstructionElement.Designation));
                // }
            }
            foreach (var profile in TestData.profiles)
            {
                _mockProfileRepo.Setup(mock =>
                    mock.GetById(profile.Id)).Returns(
                        TestData.profiles.SingleOrDefault(
                            v => v.Id == profile.Id));
            }
            foreach (var steel in TestData.steel)
            {
                _mockSteelRepo.Setup(mock =>
                    mock.GetById(steel.Id)).Returns(
                        TestData.steel.SingleOrDefault(
                            v => v.Id == steel.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<ConstructionElement>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<ConstructionElement>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<ConstructionElement>())).Verifiable();

            _service = new ConstructionElementService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockConstructionRepo.Object,
                _mockProfileRepo.Object,
                _mockSteelRepo.Object);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnConstructionElements()
        {
            // Arrange
            int constructionId = _rnd.Next(1, _maxConstructionId);

            // Act
            var returnedConstructionElements = _service.GetAllByConstructionId(
                constructionId);

            // Assert
            Assert.Equal(_constructionElements.Where(
                v => v.Construction.Id == constructionId), returnedConstructionElements);
        }

        [Fact]
        public void GetAllByConstructionId_ShouldReturnEmptyArray_WhenWrongConstructionId()
        {
            // Act
            var returnedConstructionElements = _service.GetAllByConstructionId(999);

            // Assert
            Assert.Empty(returnedConstructionElements);
        }

        [Fact]
        public void Create_ShouldCreateConstructionElement()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.marks.Count());
            int profileId = _rnd.Next(1, TestData.profiles.Count());
            int steelId = _rnd.Next(1, TestData.steel.Count());

            var newConstructionElement = new ConstructionElement
            {
                Length = 1.0f,
            };

            // Act
            _service.Create(
                newConstructionElement,
                constructionId,
                profileId,
                steelId);

            // Assert
            _repository.Verify(mock => mock.Add(
                It.IsAny<ConstructionElement>()), Times.Once);
            Assert.NotNull(newConstructionElement.Construction);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int constructionId = _rnd.Next(1, TestData.marks.Count());
            int profileId = _rnd.Next(1, TestData.profiles.Count());
            int steelId = _rnd.Next(1, TestData.steel.Count());

            var newConstructionElement = new ConstructionElement
            {
                Length = 1.0f,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null,
                constructionId,
                profileId,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                999,
                profileId,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                constructionId,
                999,
                steelId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newConstructionElement,
                constructionId,
                profileId,
                999));

            _repository.Verify(mock => mock.Add(It.IsAny<ConstructionElement>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictConstructionId = _constructionElements[0].Mark.Id;
        //     var conflictDesignation = _constructionElements[0].Designation;

        //     var newConstructionElement = new ConstructionElement
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewCreate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Create(newConstructionElement, conflictConstructionId));

        //     _repository.Verify(mock => mock.Add(It.IsAny<ConstructionElement>()), Times.Never);
        // }

        [Fact]
        public void Update_ShouldUpdateConstructionElement()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionElements.Count());
            int profileId = _rnd.Next(1, TestData.profiles.Count());
            int steelId = _rnd.Next(1, TestData.steel.Count());
            var newFloatValue = 9.0f;

            var newConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                ProfileId = profileId,
                SteelId = steelId,
                Length = newFloatValue,
            };

            // Act
            _service.Update(id, newConstructionElementRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<ConstructionElement>()), Times.Once);
            Assert.Equal(TestData.profiles.SingleOrDefault(v => v.Id == profileId), _constructionElements.SingleOrDefault(
                v => v.Id == id).Profile);
            Assert.Equal(TestData.steel.SingleOrDefault(v => v.Id == steelId), _constructionElements.SingleOrDefault(
                v => v.Id == id).Steel);
            Assert.Equal(newFloatValue, _constructionElements.SingleOrDefault(
                v => v.Id == id).Length);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionElements.Count());

            var newConstructionElementRequest = new ConstructionElementUpdateRequest
            {
                Length = 9,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, newConstructionElementRequest));

            _repository.Verify(mock => mock.Update(
                It.IsAny<ConstructionElement>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     var conflictDesignation = _constructionElements[0].Designation;
        //     var id = _constructionElements[3].Id;

        //     var newConstructionElementRequest = new ConstructionElementUpdateRequest
        //     {
        //         Designation = conflictDesignation,
        //         Name = "NewUpdate",
        //     };

        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newConstructionElementRequest));

        //     _repository.Verify(mock => mock.Update(
        //         It.IsAny<ConstructionElement>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteConstructionElement()
        {
            // Arrange
            int id = _rnd.Next(1, _constructionElements.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionElement>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(
                It.IsAny<ConstructionElement>()), Times.Never);
        }
    }
}
