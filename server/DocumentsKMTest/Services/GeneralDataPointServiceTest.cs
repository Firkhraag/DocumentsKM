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
        private readonly Mock<IGeneralDataPointRepo> _repository = new Mock<IGeneralDataPointRepo>();
        private readonly Mock<IUserRepo> _mockUserRepo = new Mock<IUserRepo>();
        private readonly Mock<IGeneralDataSectionRepo> _mockGeneralDataSectionRepo = new Mock<IGeneralDataSectionRepo>();
        private readonly IGeneralDataPointService _service;
        private readonly Random _rnd = new Random();
        private readonly List<GeneralDataPoint> _generalDataPoints = new List<GeneralDataPoint> { };
        private readonly int _maxUserId = 3;

        public GeneralDataPointServiceTest()
        {
            // Arrange
            foreach (var gdp in TestData.generalDataPoints)
            {
                _generalDataPoints.Add(new GeneralDataPoint
                {
                    Id = gdp.Id,
                    User = gdp.User,
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
                _mockUserRepo.Setup(mock =>
                    mock.GetById(user.Id)).Returns(
                        TestData.users.SingleOrDefault(v => v.Id == user.Id));

                foreach (var generalDataSection in TestData.generalDataSections)
                {
                    _repository.Setup(mock =>
                    mock.GetAllByUserAndSectionId(user.Id, generalDataSection.Id)).Returns(
                        _generalDataPoints.Where(
                            v => v.User.Id == user.Id && v.Section.Id == generalDataSection.Id));

                    foreach (var generalDataPoint in _generalDataPoints)
                    {
                        _repository.Setup(mock =>
                            mock.GetByUniqueKey(
                                user.Id, generalDataSection.Id, generalDataPoint.Text)).Returns(
                                    _generalDataPoints.SingleOrDefault(
                                        v => v.User.Id == user.Id && v.Section.Id == generalDataSection.Id &&
                                            v.Text == generalDataPoint.Text));
                    }
                }
            }
            foreach (var generalDataSection in TestData.generalDataSections)
            {
                _mockGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(generalDataSection.Id)).Returns(
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
                _mockUserRepo.Object,
                _mockGeneralDataSectionRepo.Object);
        }

        [Fact]
        public void GetAllByUserAndSectionId_ShouldReturnuserGeneralDataPoints()
        {
            // Arrange
            int userId = _rnd.Next(1, _maxUserId);
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints = _service.GetAllByUserAndSectionId(userId, sectionId);

            // Assert
            Assert.Equal(_generalDataPoints.Where(
                v => v.User.Id == userId && v.Section.Id == sectionId),
                    returnedGeneralDataPoints);
        }

        [Fact]
        public void GetAllByUserAndSectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int userId = _rnd.Next(1, _maxUserId);
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedGeneralDataPoints1 = _service.GetAllByUserAndSectionId(999, sectionId);
            var returnedGeneralDataPoints2 = _service.GetAllByUserAndSectionId(userId, 999);

            // Assert
            Assert.Empty(returnedGeneralDataPoints1);
            Assert.Empty(returnedGeneralDataPoints2);
        }

        [Fact]
        public void Create_ShouldCreateGeneralDataPoint()
        {
            // Arrange
            int userId = _rnd.Next(1, TestData.users.Count());
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act
            _service.Create(newGeneralDataPoint, userId, sectionId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Once);
            Assert.NotNull(newGeneralDataPoint.User);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int userId = _rnd.Next(1, TestData.users.Count());
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, userId, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newGeneralDataPoint, userId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int conflictuserId = _generalDataPoints[0].User.Id;
            int conflictSectionId = _generalDataPoints[0].Section.Id;

            var newGeneralDataPoint = new GeneralDataPoint
            {
                Text = _generalDataPoints[0].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(
                newGeneralDataPoint, conflictuserId, conflictSectionId));

            _repository.Verify(mock => mock.Add(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateuserGeneralDataPoint()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int userId = _generalDataPoints[0].User.Id;
            int sectionId = _generalDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act
            _service.Update(
                id, userId, sectionId, newGeneralDataPointRequest);

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
            int userId = _generalDataPoints[0].User.Id;
            int sectionId = _generalDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, userId, sectionId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, userId, sectionId, newGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, 999, sectionId, newGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, userId, 999, newGeneralDataPointRequest));


            _repository.Verify(mock => mock.Update(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int userId = _generalDataPoints[0].User.Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            var newGeneralDataPointRequest = new GeneralDataPointUpdateRequest
            {
                Text = _generalDataPoints[6].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(
                id, userId, sectionId, newGeneralDataPointRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<GeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteuserGeneralDataPoint()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int userId = _generalDataPoints[0].User.Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            // Act
            _service.Delete(id, userId, sectionId);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<GeneralDataPoint>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            int id = _generalDataPoints[0].Id;
            int userId = _generalDataPoints[0].User.Id;
            int sectionId = _generalDataPoints[0].Section.Id;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999, userId, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, 999, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, userId, 999));

            _repository.Verify(mock => mock.Delete(It.IsAny<GeneralDataPoint>()), Times.Never);
        }
    }
}
