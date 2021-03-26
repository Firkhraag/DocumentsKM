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
        private readonly IMarkGeneralDataPointService _service;
        private readonly Random _rnd = new Random();
        private readonly List<MarkGeneralDataPoint> _markGeneralDataPoints = new List<MarkGeneralDataPoint> { };

        public MarkGeneralDataPointServiceTest()
        {
            var mockMarkRepo = new Mock<IMarkRepo>();
            var mockMarkGeneralDataSectionRepo = new Mock<IMarkGeneralDataSectionRepo>();
            var mockGeneralDataPointRepo = new Mock<IGeneralDataPointRepo>();

            // Arrange
            foreach (var mgdp in TestData.markGeneralDataPoints)
            {
                _markGeneralDataPoints.Add(new MarkGeneralDataPoint
                {
                    Id = mgdp.Id,
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
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _markGeneralDataPoints.Where(v => v.Section.Mark.Id == mark.Id));

                foreach (var generalDataSection in TestData.markGeneralDataSections)
                {
                    foreach (var markGeneralDataPoint in _markGeneralDataPoints)
                    {
                        
                    }
                }
            }
            foreach (var markGeneralDataSection in TestData.markGeneralDataSections)
            {
                mockMarkGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(markGeneralDataSection.Id, true)).Returns(
                        TestData.markGeneralDataSections.SingleOrDefault(
                            v => v.Id == markGeneralDataSection.Id));
                mockMarkGeneralDataSectionRepo.Setup(mock =>
                    mock.GetById(markGeneralDataSection.Id, false)).Returns(
                        TestData.markGeneralDataSections.SingleOrDefault(
                            v => v.Id == markGeneralDataSection.Id));

                _repository.Setup(mock =>
                    mock.GetAllBySectionId(markGeneralDataSection.Id)).Returns(
                        _markGeneralDataPoints.Where(
                            v => v.Section.Id == markGeneralDataSection.Id));

                foreach (var markGeneralDataPoint in _markGeneralDataPoints)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(
                            markGeneralDataSection.Id, markGeneralDataPoint.Text)).Returns(
                                _markGeneralDataPoints.SingleOrDefault(
                                    v => v.Section.Id == markGeneralDataSection.Id &&
                                        v.Text == markGeneralDataPoint.Text));
                }
            }
            foreach (var generalDataPoint in TestData.generalDataPoints)
            {
                mockGeneralDataPointRepo.Setup(mock =>
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
                mockMarkRepo.Object,
                mockMarkGeneralDataSectionRepo.Object,
                mockGeneralDataPointRepo.Object);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnMarkGeneralDataPoints()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints = _service.GetAllBySectionId(sectionId);

            // Assert
            Assert.Equal(_markGeneralDataPoints.Where(
                v => v.Section.Id == sectionId), returnedMarkGeneralDataPoints);
        }

        [Fact]
        public void GetAllBySectionId_ShouldReturnEmptyArray_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            // Act
            var returnedMarkGeneralDataPoints = _service.GetAllBySectionId(999);

            // Assert
            Assert.Empty(returnedMarkGeneralDataPoints);
        }

        [Fact]
        public void Create_ShouldCreateMarkGeneralDataPoint()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act
            _service.Create(newMarkGeneralDataPoint, sectionId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
            Assert.NotNull(newMarkGeneralDataPoint.Section);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int sectionId = _rnd.Next(1, TestData.markGeneralDataSections.Count());

            var newmarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newmarkGeneralDataPoint, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int conflictSectionId = _markGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPoint = new MarkGeneralDataPoint
            {
                Text = _markGeneralDataPoints[0].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newMarkGeneralDataPoint, conflictSectionId));

            _repository.Verify(mock => mock.Add(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdatemarkGeneralDataPoint()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act
            _service.Update(
                id, sectionId, newMarkGeneralDataPointRequest);

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
            int sectionId = _markGeneralDataPoints[0].Section.Id;
            var newStringValue = "NewUpdate";

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = newStringValue,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, sectionId, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, sectionId, newMarkGeneralDataPointRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                id, 999, newMarkGeneralDataPointRequest));


            _repository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            var newMarkGeneralDataPointRequest = new MarkGeneralDataPointUpdateRequest
            {
                Text = _markGeneralDataPoints[6].Text,
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(
                id, sectionId, newMarkGeneralDataPointRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeletemarkGeneralDataPoint()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act
            _service.Delete(id, sectionId);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Arrange
            int id = _markGeneralDataPoints[0].Id;
            int sectionId = _markGeneralDataPoints[0].Section.Id;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999, sectionId));
            Assert.Throws<ArgumentNullException>(() => _service.Delete(id, 999));

            _repository.Verify(mock => mock.Delete(It.IsAny<MarkGeneralDataPoint>()), Times.Never);
        }
    }
}
