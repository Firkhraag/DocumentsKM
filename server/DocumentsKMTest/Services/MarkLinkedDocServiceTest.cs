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
    public class MarkLinkedDocServiceTest
    {
        private readonly Mock<IMarkLinkedDocRepo> _mockMarkLinkedDocRepo = new Mock<IMarkLinkedDocRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<ILinkedDocRepo> _mockLinkedDocRepo = new Mock<ILinkedDocRepo>();
        private readonly MarkLinkedDocService _service;
        private readonly Random _rnd = new Random();

        public MarkLinkedDocServiceTest()
        {
            // Arrange
            foreach (var markLinkedDoc in TestData.markLinkedDocs)
            {
                _mockMarkLinkedDocRepo.Setup(mock=>
                    mock.GetById(markLinkedDoc.Id)).Returns(
                        TestData.markLinkedDocs.SingleOrDefault(v => v.Id == markLinkedDoc.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                _mockMarkLinkedDocRepo.Setup(mock=>
                    mock.GetAllByMarkId(mark.Id)).Returns(
                        TestData.markLinkedDocs.Where(v => v.Mark.Id == mark.Id));

                // foreach (var markLinkedDoc in TestData.markLinkedDocs)
                // {
                //     _mockMarkLinkedDocRepo.Setup(mock=>
                //         mock.GetByUniqueKeyValues(mark.Id, MarkLinkedDoc.Designation)).Returns(
                //             TestData.MarkLinkedDocs.SingleOrDefault(
                //                 v => v.Mark.Id == mark.Id && v.Designation == MarkLinkedDoc.Designation));
                // }
            }

            _mockMarkLinkedDocRepo.Setup(mock=>
                mock.Add(It.IsAny<MarkLinkedDoc>())).Verifiable();
            _mockMarkLinkedDocRepo.Setup(mock=>
                mock.Update(It.IsAny<MarkLinkedDoc>())).Verifiable();
            _mockMarkLinkedDocRepo.Setup(mock=>
                mock.Delete(It.IsAny<MarkLinkedDoc>())).Verifiable();

            _service = new MarkLinkedDocService(
                _mockMarkLinkedDocRepo.Object,
                _mockMarkRepo.Object,
                _mockLinkedDocRepo.Object);
        }

        [Fact]
        public void GetAllByMarkId_ShouldReturnAllMarkLinkedDocs()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            
            // Act
            var returnedMarkLinkedDocs = _service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.markLinkedDocs.Where(
                v => v.Mark.Id == markId), returnedMarkLinkedDocs);
        }

        [Fact]
        public void Create_ShouldCreateMarkLinkedDoc()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            var linkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            var newMarkLinkedDoc = new MarkLinkedDoc{};

            // Act
            _service.Add(newMarkLinkedDoc, markId, linkedDocId);

            // Assert
            _mockMarkLinkedDocRepo.Verify(mock => mock.Add(It.IsAny<MarkLinkedDoc>()), Times.Once);
            Assert.NotNull(newMarkLinkedDoc.LinkedDoc);
            Assert.NotNull(newMarkLinkedDoc.Mark);
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int wrongMarkId = 999;
            var linkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            int wrongLinkedDocId = 999;

            var newMarkLinkedDoc = new MarkLinkedDoc{};
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Add(null, markId, linkedDocId));
            Assert.Throws<ArgumentNullException>(() => _service.Add(newMarkLinkedDoc, wrongMarkId, linkedDocId));
            Assert.Throws<ArgumentNullException>(() => _service.Add(newMarkLinkedDoc, markId, wrongLinkedDocId));

            _mockMarkLinkedDocRepo.Verify(mock => mock.Add(It.IsAny<MarkLinkedDoc>()), Times.Never);
        }

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     // Possible conflict values
        //     var conflictMarkId = TestData.MarkLinkedDocs[0].Mark.Id;
        //     var conflictDesignation = TestData.MarkLinkedDocs[0].Designation;

        //     var newMarkLinkedDoc = new MarkLinkedDoc
        //     {
        //         Designation=conflictDesignation,
        //         Name="NewCreate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Create(newMarkLinkedDoc, conflictMarkId));

        //     _mockMarkLinkedDocRepo.Verify(mock => mock.Add(It.IsAny<MarkLinkedDoc>()), Times.Never);
        // }
        
        [Fact]
        public void Update_ShouldUpdateMarkLinkedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.markLinkedDocs.Count());
            var newLinkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            while (newLinkedDocId == TestData.markLinkedDocs[id].LinkedDoc.Id)
            {
                newLinkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            }

            var newMarkLinkedDocRequest = new MarkLinkedDocRequest
            {
                LinkedDocId=newLinkedDocId,
            };
            
            // Act
            _service.Update(id, newMarkLinkedDocRequest);

            // Assert
            _mockMarkLinkedDocRepo.Verify(mock => mock.Update(It.IsAny<MarkLinkedDoc>()), Times.Once);
            Assert.Equal(newLinkedDocId, TestData.markLinkedDocs.SingleOrDefault(v => v.Id == id).LinkedDoc.Id);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.markLinkedDocs.Count());
            var newLinkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            while (newLinkedDocId == TestData.markLinkedDocs[id].LinkedDoc.Id)
            {
                newLinkedDocId = _rnd.Next(1, TestData.linkedDocs.Count());
            }
            int wrongId = 999;
            int wrongLinkedDocId = 999;

            var newMarkLinkedDocRequest = new MarkLinkedDocRequest
            {
                LinkedDocId=newLinkedDocId,
            };
            var wrongMarkLinkedDocRequest = new MarkLinkedDocRequest
            {
                LinkedDocId=wrongLinkedDocId,
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(wrongId, newMarkLinkedDocRequest));
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, wrongMarkLinkedDocRequest));
            _mockMarkLinkedDocRepo.Verify(mock => mock.Update(It.IsAny<MarkLinkedDoc>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     // Possible conflict values
        //     var conflictMarkId = TestData.MarkLinkedDocs[0].Mark.Id;
        //     var conflictDesignation = TestData.MarkLinkedDocs[0].Designation;
        //     var id = TestData.MarkLinkedDocs[3].Id;

        //     var newMarkLinkedDocRequest = new MarkLinkedDocUpdateRequest
        //     {
        //         Designation=conflictDesignation,
        //         Name="NewUpdate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newMarkLinkedDocRequest));

        //     _mockMarkLinkedDocRepo.Verify(mock => mock.Update(It.IsAny<MarkLinkedDoc>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteMarkLinkedDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.markLinkedDocs.Count());
            
            // Act
            _service.Delete(id);

            // Assert
            _mockMarkLinkedDocRepo.Verify(mock => mock.Delete(It.IsAny<MarkLinkedDoc>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Arrange
            var wrongId = 999;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(wrongId));

            _mockMarkLinkedDocRepo.Verify(mock => mock.Delete(It.IsAny<MarkLinkedDoc>()), Times.Never);
        }
    }
}
