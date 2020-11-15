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
        [Fact]
        public void GetAll_ShouldReturnAllAttachedDocs()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var rnd = new Random();
            int markId = rnd.Next(1, TestData.marks.Count());

            mockAttachedDocRepo.Setup(mock=>
                    mock.GetAllByMarkId(markId)).Returns(
                        TestData.attachedDocs.Where(v => v.Mark.Id == markId));

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);
            
            // Act
            var returnedAttachedDocs = service.GetAllByMarkId(markId);

            // Assert
            Assert.Equal(TestData.attachedDocs.Where(
                v => v.Mark.Id == markId), returnedAttachedDocs);
        }

        [Fact]
        public void Create_ShouldCreateAttachedDoc()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var rnd = new Random();
            int markId = rnd.Next(1, TestData.marks.Count());

            mockAttachedDocRepo.Setup(mock=>
                mock.Add(It.IsAny<AttachedDoc>())).Verifiable();

            markRepo.Setup(mock=>
                mock.GetById(markId)).Returns(
                    TestData.marks.SingleOrDefault(v => v.Id == markId));

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDoc = new AttachedDoc{
                Designation="New",
                Name="New",
            };
            
            // Act
            service.Create(newAttachedDoc, markId);

            // Assert
            mockAttachedDocRepo.Verify();
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            foreach (var mark in TestData.marks)
            {
                markRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));
            }

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDoc = new AttachedDoc{
                Designation="New",
                Name="New",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Create(
                null, TestData.marks[0].Id));
            Assert.Throws<ArgumentNullException>(() => service.Create(newAttachedDoc, 999));
        }

        [Fact]
        public void Create_ShouldFailWithConflict()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var conflictMarkId = TestData.attachedDocs[0].Mark.Id;
            var conflictDesignation = TestData.attachedDocs[0].Designation;

            markRepo.Setup(mock=>
                mock.GetById(conflictMarkId)).Returns(TestData.marks.SingleOrDefault(v => v.Id == conflictMarkId));
            mockAttachedDocRepo.Setup(mock=>
                mock.GetByUniqueKeyValues(conflictMarkId, conflictDesignation)).Returns(
                    TestData.attachedDocs.SingleOrDefault(v => v.Mark.Id == conflictMarkId && v.Designation == conflictDesignation));

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDoc = new AttachedDoc{
                Designation=conflictDesignation,
                Name="New",
            };
            
            // Act & Assert
            Assert.Throws<ConflictException>(() => service.Create(newAttachedDoc, conflictMarkId));
        }
        
        [Fact]
        public void Update_ShouldUpdateAttachedDoc()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var rnd = new Random();
            int id = rnd.Next(1, TestData.attachedDocs.Count());

            mockAttachedDocRepo.Setup(mock=>
                mock.GetById(id)).Returns(TestData.attachedDocs.SingleOrDefault(v => v.Id == id));

            mockAttachedDocRepo.Setup(mock=>
                mock.Update(It.IsAny<AttachedDoc>())).Verifiable();

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDocRequest = new AttachedDocUpdateRequest{
                Designation="New",
                Name="New",
            };
            
            // Act
            service.Update(id, newAttachedDocRequest);

            // Assert
            mockAttachedDocRepo.Verify();
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            foreach (var attachedDoc in TestData.attachedDocs)
            {
                mockAttachedDocRepo.Setup(mock=>
                    mock.GetById(attachedDoc.Id)).Returns(
                        TestData.attachedDocs.SingleOrDefault(v => v.Id == attachedDoc.Id));
            }

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDocRequest = new AttachedDocUpdateRequest{
                Designation="New",
                Name="New",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Update(
                TestData.attachedDocs[0].Id, null));
            Assert.Throws<ArgumentNullException>(() => service.Update(999, newAttachedDocRequest));
        }

        [Fact]
        public void Update_ShouldFailWithConflict()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var conflictMarkId = TestData.attachedDocs[0].Mark.Id;
            var conflictDesignation = TestData.attachedDocs[0].Designation;
            // Possible conflict id
            var id = TestData.attachedDocs[3].Id;

            mockAttachedDocRepo.Setup(mock=>
                mock.GetById(id)).Returns(
                    TestData.attachedDocs.SingleOrDefault(v => v.Id == id));

            mockAttachedDocRepo.Setup(mock=>
                mock.GetByUniqueKeyValues(conflictMarkId, conflictDesignation)).Returns(
                    TestData.attachedDocs.SingleOrDefault(
                        v => v.Mark.Id == conflictMarkId && v.Designation == conflictDesignation));

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            var newAttachedDocRequest = new AttachedDocUpdateRequest{
                Designation=conflictDesignation,
                Name="New",
            };
            
            // Act & Assert
            Assert.Throws<ConflictException>(() => service.Update(id, newAttachedDocRequest));
        }

        [Fact]
        public void Delete_ShouldDeleteAttachedDoc()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            var rnd = new Random();
            int attachedDocId = rnd.Next(1, TestData.attachedDocs.Count());

            mockAttachedDocRepo.Setup(mock=>
                mock.Delete(It.IsAny<AttachedDoc>())).Verifiable();
            mockAttachedDocRepo.Setup(mock=>
                mock.GetById(attachedDocId)).Returns(
                    TestData.attachedDocs[attachedDocId]);

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);
            
            // Act
            service.Delete(attachedDocId);

            // Assert
            mockAttachedDocRepo.Verify();
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Arrange
            var mockAttachedDocRepo = new Mock<IAttachedDocRepo>();
            var markRepo = new Mock<IMarkRepo>();

            foreach (var attachedDoc in TestData.attachedDocs)
            {
                mockAttachedDocRepo.Setup(mock=>
                    mock.GetById(attachedDoc.Id)).Returns(
                        TestData.attachedDocs.SingleOrDefault(v => v.Id == attachedDoc.Id));
            }

            var service = new AttachedDocService(
                mockAttachedDocRepo.Object,
                markRepo.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.Delete(999));
        }
    }
}
