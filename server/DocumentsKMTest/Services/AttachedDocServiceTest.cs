using System;
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
        private readonly Mock<IAttachedDocRepo> _mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly AttachedDocService _service;
        private readonly Random _rnd = new Random();

        public AttachedDocServiceTest()
        {
            // Arrange
            foreach (var attachedDoc in TestData.attachedDocs)
            {
                _mockAttachedDocRepo.Setup(mock=>
                    mock.GetById(attachedDoc.Id)).Returns(
                        TestData.attachedDocs.SingleOrDefault(v => v.Id == attachedDoc.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _mockAttachedDocRepo.Setup(mock=>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        TestData.attachedDocs.Where(v => v.Mark.Id == mark.Id));

                foreach (var attachedDoc in TestData.attachedDocs)
                {
                    _mockAttachedDocRepo.Setup(mock=>
                        mock.GetByUniqueKeyValues(mark.Id, attachedDoc.Designation)).Returns(
                            TestData.attachedDocs.SingleOrDefault(
                                v => v.Mark.Id == mark.Id && v.Designation == attachedDoc.Designation));
                }
            }

            _mockAttachedDocRepo.Setup(mock=>
                mock.Add(It.IsAny<AttachedDoc>())).Verifiable();
            _mockAttachedDocRepo.Setup(mock=>
                mock.Update(It.IsAny<AttachedDoc>())).Verifiable();
            _mockAttachedDocRepo.Setup(mock=>
                mock.Delete(It.IsAny<AttachedDoc>())).Verifiable();

            _service = new AttachedDocService(
                _mockAttachedDocRepo.Object,
                _mockMarkRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAllAttachedDocs()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            
            // Act
            var returnedAttachedDocs = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.attachedDocs.Where(
                v => v.Mark.Id == markId), returnedAttachedDocs);
        }

        [Fact]
        public void Create_ShouldCreateAttachedDoc()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());

            var newAttachedDoc = new AttachedDoc
            {
                Designation="NewCreate",
                Name="NewCreate",
            };
            
            // Act
            _service.Create(newAttachedDoc, markId);

            // Assert
            _mockAttachedDocRepo.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Once);
            Assert.NotNull(newAttachedDoc.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int wrongMarkId = 999;

            var newAttachedDoc = new AttachedDoc
            {
                Designation="NewCreate",
                Name="NewCreate",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(null, markId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(newAttachedDoc, wrongMarkId));

            _mockAttachedDocRepo.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Create_ShouldFailWithConflict()
        {
            // Arrange
            // Possible conflict values
            var conflictMarkId = TestData.attachedDocs[0].Mark.Id;
            var conflictDesignation = TestData.attachedDocs[0].Designation;

            var newAttachedDoc = new AttachedDoc
            {
                Designation=conflictDesignation,
                Name="NewCreate",
            };
            
            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Create(newAttachedDoc, conflictMarkId));

            _mockAttachedDocRepo.Verify(mock => mock.Add(It.IsAny<AttachedDoc>()), Times.Never);
        }
        
        [Fact]
        public void Update_ShouldUpdateAttachedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.attachedDocs.Count());
            var newDesignation = "NewUpdate";

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation=newDesignation,
            };
            
            // Act
            _service.Update(id, newAttachedDocRequest);

            // Assert
            _mockAttachedDocRepo.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Once);
            Assert.Equal(newDesignation, TestData.attachedDocs.SingleOrDefault(v => v.Id == id).Designation);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.attachedDocs.Count());
            int wrongId = 999;

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation="NewUpdate",
                Name="NewUpdate",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(wrongId, newAttachedDocRequest));

            _mockAttachedDocRepo.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            // Possible conflict values
            var conflictMarkId = TestData.attachedDocs[0].Mark.Id;
            var conflictDesignation = TestData.attachedDocs[0].Designation;
            var id = TestData.attachedDocs[3].Id;

            var newAttachedDocRequest = new AttachedDocUpdateRequest
            {
                Designation=conflictDesignation,
                Name="NewUpdate",
            };
            
            // Act & Assert
            Assert.Throws<ConflictException>(() => _service.Update(id, newAttachedDocRequest));

            _mockAttachedDocRepo.Verify(mock => mock.Update(It.IsAny<AttachedDoc>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteAttachedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.attachedDocs.Count());
            
            // Act
            _service.Delete(id);

            // Assert
            _mockAttachedDocRepo.Verify(mock => mock.Delete(It.IsAny<AttachedDoc>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Arrange
            var wrongId = 999;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(wrongId));

            _mockAttachedDocRepo.Verify(mock => mock.Delete(It.IsAny<AttachedDoc>()), Times.Never);
        }
    }
}
