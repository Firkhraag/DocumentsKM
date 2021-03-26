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
    public class AttachedDocServiceTest
    {
        private readonly Mock<IAttachedDocRepo> _repository = new Mock<IAttachedDocRepo>();
        private readonly IAttachedDocService _service;
        private readonly Random _rnd = new Random();
        private readonly List<AttachedDoc> _attachedDocs = new List<AttachedDoc> { };
        private readonly int _maxMarkId = 3;

        public AttachedDocServiceTest()
        {
            var mockMarkRepo = new Mock<IMarkRepo>();

            // Arrange
            foreach (var attachedDoc in TestData.attachedDocs)
            {
                _attachedDocs.Add(new AttachedDoc
                {
                    Id = attachedDoc.Id,
                    Mark = attachedDoc.Mark,
                    Designation = attachedDoc.Designation,
                    Name = attachedDoc.Name,
                    Note = attachedDoc.Note,
                });
            }
            foreach (var attachedDoc in _attachedDocs)
            {
                _repository.Setup(mock =>
                    mock.GetById(attachedDoc.Id)).Returns(
                        _attachedDocs.SingleOrDefault(v => v.Id == attachedDoc.Id));
            }
            foreach (var mark in TestData.marks)
            {
                mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _repository.Setup(mock =>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        _attachedDocs.Where(v => v.Mark.Id == mark.Id));

                foreach (var attachedDoc in _attachedDocs)
                {
                    _repository.Setup(mock =>
                        mock.GetByUniqueKey(mark.Id, attachedDoc.Designation)).Returns(
                            _attachedDocs.SingleOrDefault(
                                v => v.Mark.Id == mark.Id && v.Designation == attachedDoc.Designation));
                }
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<AttachedDoc>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<AttachedDoc>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<AttachedDoc>())).Verifiable();

            _service = new AttachedDocService(
                _repository.Object,
                mockMarkRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAttachedDocs()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);

            // Act
            var returnedAttachedDocs = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(_attachedDocs.Where(
                v => v.Mark.Id == markId), returnedAttachedDocs);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Act
            var returnedAttachedDocs = _service.GetAllByMarkId(999);

            // Assert
            Assert.Empty(returnedAttachedDocs);
        }

        [Fact]
        public void Create_ShouldCreateAttachedDoc()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());

            var newAttachedDoc = new AttachedDoc
            {
                Designation = "NewCreate",
                Name = "NewCreate",
            };

            // Act
            _service.Create(newAttachedDoc, markId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Once);
            Assert.NotNull(newAttachedDoc.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());

            var newAttachedDoc = new AttachedDoc
            {
                Designation = "NewCreate",
                Name = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, markId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newAttachedDoc, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            var conflictMarkId = _attachedDocs[0].Mark.Id;
            var conflictDesignation = _attachedDocs[0].Designation;

            var newAttachedDoc = new AttachedDoc
            {
                Designation = conflictDesignation,
                Name = "NewCreate",
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newAttachedDoc, conflictMarkId));

            _repository.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateAttachedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, _attachedDocs.Count());
            var newStringValue = "NewUpdate";

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation = newStringValue,
                Name = newStringValue,
                Note = newStringValue,
            };

            // Act
            _service.Update(id, newAttachedDocRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Once);
            var v = _attachedDocs.SingleOrDefault(v => v.Id == id);
            Assert.Equal(newStringValue, v.Designation);
            Assert.Equal(newStringValue, v.Name);
            Assert.Equal(newStringValue, v.Note);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _attachedDocs.Count());

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(999, newAttachedDocRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict_WhenConflictValues()
        {
            // Arrange
            var conflictDesignation = _attachedDocs[0].Designation;
            var id = _attachedDocs[3].Id;

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation = conflictDesignation,
                Name = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id, newAttachedDocRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteAttachedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, _attachedDocs.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<AttachedDoc>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(It.IsAny<AttachedDoc>()), Times.Never);
        }
    }
}
