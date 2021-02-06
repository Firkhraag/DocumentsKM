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
    public class MarkGeneralDataPointServiceTest
    {
        private readonly Mock<IMarkGeneralDataPointRepo> _repository = new Mock<IMarkGeneralDataPointRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IGeneralDataSectionRepo> _mockGeneralDataSectionRepo = new Mock<IGeneralDataSectionRepo>();
        private readonly Mock<IGeneralDataPointRepo> _mockGeneralDataPointRepo = new Mock<IGeneralDataPointRepo>();
        private readonly IMarkGeneralDataPointService _service;
        private readonly Random _rnd = new Random();
        private readonly List<MarkGeneralDataPoint> _markGeneralDataPoints = new List<MarkGeneralDataPoint> { };
        private readonly int _maxMarkId = 3;

        public MarkGeneralDataPointServiceTest()
        {
            // Arrange
            foreach (var mgdp in TestData.markGeneralDataPoints)
            {
                _markGeneralDataPoints.Add(new MarkGeneralDataPoint
                {
                    Id = mgdp.Id,
                    Mark = mgdp.Mark,
                    Section = mgdp.Section,
                    Text = mgdp.Text,
                    OrderNum = mgdp.OrderNum,
                });
            }
            foreach (var markGeneralDataPoint in _markGeneralDataPoints)
            {
                _repository.Setup(mock =>
                    mock.GetById(markGeneralDataPoint.Id)).Returns(
                        _markGeneralDataPoints.SingleOrDefault(v => v.Id == markGeneralDataPoint.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _markGeneralDataPoints.Where(v => v.Mark.Id == mark.Id));

                foreach (var generalDataSection in TestData.generalDataSections)
                {
                    _repository.Setup(mock =>
                    mock.GetAllByMarkAndSectionId(mark.Id, generalDataSection.Id)).Returns(
                        _markGeneralDataPoints.Where(
                            v => v.Mark.Id == mark.Id && v.Section.Id == generalDataSection.Id));

                    foreach (var markGeneralDataPoint in _markGeneralDataPoints)
                    {
                        _repository.Setup(mock =>
                            mock.GetByUniqueKey(
                                mark.Id, generalDataSection.Id, markGeneralDataPoint.Text)).Returns(
                                    _markGeneralDataPoints.SingleOrDefault(
                                        v => v.Mark.Id == mark.Id && v.Section.Id == generalDataSection.Id &&
                                            v.Text == markGeneralDataPoint.Text));
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
            foreach (var generalDataPoint in TestData.generalDataPoints)
            {
                _mockGeneralDataPointRepo.Setup(mock =>
                    mock.GetById(generalDataPoint.Id)).Returns(
                        TestData.generalDataPoints.SingleOrDefault(
                            v => v.Id == generalDataPoint.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<MarkGeneralDataPoint>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<MarkGeneralDataPoint>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<MarkGeneralDataPoint>())).Verifiable();

            _service = new MarkGeneralDataPointService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockGeneralDataSectionRepo.Object,
                _mockGeneralDataPointRepo.Object);
        }

        [Fact]
        public void GetAllByMarkAndSectionId_ShouldReturnMarkGeneralDataPoints()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints = _service.GetAllByMarkAndSectionId(markId, sectionId);

            // Assert
            Assert.Equal(_markGeneralDataPoints.Where(
                v => v.Mark.Id == markId && v.Section.Id == sectionId),
                    returnedMarkGeneralDataPoints);
        }

        [Fact]
        public void GetAllByMarkAndSectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints1 = _service.GetAllByMarkAndSectionId(999, sectionId);
            var returnedMarkGeneralDataPoints2 = _service.GetAllByMarkAndSectionId(markId, 999);

            // Assert
            Assert.Empty(returnedMarkGeneralDataPoints1);
            Assert.Empty(returnedMarkGeneralDataPoints2);
        }

        [Fact]
        public void Create_ShouldCreateMarkGeneralDataPoint()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act
            _service.Create(newMarkGeneralDataPoint, markId, sectionId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
            Assert.NotNull(newMarkGeneralDataPoint.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int sectionId = _rnd.Next(1, TestData.generalDataSections.Count());

            var newmarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, markId, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newmarkGeneralDataPoint, markId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int conflictMarkId = _markGeneralDataPoints[0].Mark.Id;
            int conflictSectionId = _markGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = _markGeneralDataPoints[0].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newMarkGeneralDataPoint, conflictMarkId, conflictSectionId));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdatemarkGeneralDataPoint()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int markId = _markGeneralDataPoints[0].Mark.Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act
            _service.Update(
                id, markId, sectionId, newMarkGeneralDataPointRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
            var v = _markGeneralDataPoints.SingleOrDefault(v => v.Id == id);
            Assert.Equal(newStringValue, v.Text);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int markId = _markGeneralDataPoints[0].Mark.Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, markId, sectionId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, markId, sectionId, newMarkGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, 999, sectionId, newMarkGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, markId, 999, newMarkGeneralDataPointRequest));


            _repository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int markId = _markGeneralDataPoints[0].Mark.Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = _markGeneralDataPoints[6].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(
                id, markId, sectionId, newMarkGeneralDataPointRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeletemarkGeneralDataPoint()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int markId = _markGeneralDataPoints[0].Mark.Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act
            _service.Delete(id, markId, sectionId);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int markId = _markGeneralDataPoints[0].Mark.Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999, markId, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, 999, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, markId, 999));

            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }
    }
}
