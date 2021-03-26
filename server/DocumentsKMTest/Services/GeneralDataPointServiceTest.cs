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
    public class GeneralDataPointServiceTest
    {
        Mock<IGeneralDataPointRepo> _repository = new Mock<IGeneralDataPointRepo>();
        private readonly IGeneralDataPointService _service;
        private readonly Random _rnd = new Random();
        private readonly List<GeneralDataPoint> _generalDataPoints = new List<GeneralDataPoint> { };

        public GeneralDataPointServiceTest()
        {
            var mockUserRepo = new Mock<IUserRepo>();
            var mockGeneralDataSectionRepo = new Mock<IGeneralDataSectionRepo>();

            // Arrange
            foreach (var gdp in TestData.generalDataPoints)
            {
                _generalDataPoints.Add(new GeneralDataPoint
                {
                    Id = gdp.Id,
                    Section = gdp.Section,
                    Text = gdp.Text,
                    OrderNum = gdp.OrderNum,
                });
            }
            foreach (var generalDataPoint in _generalDataPoints)
            {
                _repository.Setup(mock =>
                    mock.GetById(generalDataPoint.Id)).Returns(
                        _generalDataPoints.SingleOrDefault(v => v.Id == generalDataPoint.Id));
            }
            foreach (var user in TestData.users)
            {
                mockUserRepo.Setup(mock =>
                    mock.GetById(user.Id)).Returns(
                        TestData.users.SingleOrDefault(v => v.Id == user.Id));

                foreach (var generalDataSection in TestData.generalDataSections)
                {
                    foreach (var generalDataPoint in _generalDataPoints)
                    {
                        _repository.Setup(mock =>
                            mock.GetByUniqueKey(
                                generalDataSection.Id, generalDataPoint.Text)).Returns(
                                    _generalDataPoints.SingleOrDefault(
                                        v => v.Section.Id == generalDataSection.Id &&
                                            v.Text == generalDataPoint.Text));
                    }
                }
            }
            foreach (var generalDataSection in TestData.generalDataSections)
            {
                _repository.Setup(mock =>
                    mock.GetAllBySectionId(generalDataSection.Id)).Returns(
                        _generalDataPoints.Where(
                            v => v.Section.Id == generalDataSection.Id));
                mockGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(generalDataSection.Id, false)).Returns(
                        TestData.generalDataSections.SingleOrDefault(
                            v => v.Id == generalDataSection.Id));
                mockGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(generalDataSection.Id, true)).Returns(
                        TestData.generalDataSections.SingleOrDefault(
                            v => v.Id == generalDataSection.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<GeneralDataPoint>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<GeneralDataPoint>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<GeneralDataPoint>())).Verifiable();

            _service = new GeneralDataPointService(
                _repository.Object,
                mockUserRepo.Object,
                mockGeneralDataSectionRepo.Object);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnuserGeneralDataPoints()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints = _service.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_generalDataPoints.Where(
                v => v.Section.Id == sectionId),
                    returnedGeneralDataPoints);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints = _service.GetAllBySectionId(999);

            // Assert
            Assert.Empty(returnedGeneralDataPoints);
        }

        [Fact]
        public void Create_ShouldCreateGeneralDataPoint()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act
            _service.Create(newGeneralDataPoint, sectionId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Once);
            Assert.NotNull(newGeneralDataPoint.Section);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newGeneralDataPoint, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int conflictSectionId = _generalDataPoints[0].Section.Id;

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = _generalDataPoints[0].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(
                newGeneralDataPoint, conflictSectionId));

            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateuserGeneralDataPoint()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int sectionId = _generalDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act
            _service.Update(
                id, sectionId, newGeneralDataPointRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<GeneralDataPoint>()), Times.Once);
            var v = _generalDataPoints.SingleOrDefault(v => v.Id == id);
            Assert.Equal(newStringValue, v.Text);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int sectionId = _generalDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, sectionId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, sectionId, newGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, 999, newGeneralDataPointRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = _generalDataPoints[6].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(
                id, sectionId, newGeneralDataPointRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteuserGeneralDataPoint()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            // Act
            _service.Delete(id, sectionId);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<GeneralDataPoint>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, 999));

            _repository.Verify(mock => mock.Delete(It.IsAny<GeneralDataPoint>()), Times.Never);
        }
    }
}
