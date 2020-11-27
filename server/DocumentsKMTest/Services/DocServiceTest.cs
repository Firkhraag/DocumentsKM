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
    public class DocServiceTest
    {
        private readonly Mock<IDocRepo> _mockDocRepo = new Mock<IDocRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly Mock<IDocTypeRepo> _mockDocTypeRepo = new Mock<IDocTypeRepo>();
        private readonly DocService _service;
        private readonly Random _rnd = new Random();

        public DocServiceTest()
        {
            // Arrange
            foreach (var doc in TestData.docs)
            {
                _mockDocRepo.Setup(mock=>
                    mock.GetById(doc.Id)).Returns(
                        TestData.docs.SingleOrDefault(v => v.Id == doc.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock=>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                foreach (var docType in TestData.docTypes)
                {
                    _mockDocRepo.Setup(mock=>
                        mock.GetAllByMarkIdAndDocType(mark.Id, docType.Id)).Returns(
                            TestData.docs.Where(v => v.Mark.Id == mark.Id && v.Type.Id == docType.Id));

                    _mockDocRepo.Setup(mock=>
                        mock.GetAllByMarkIdAndNotDocType(mark.Id, docType.Id)).Returns(
                            TestData.docs.Where(v => v.Mark.Id == mark.Id && v.Type.Id != docType.Id));
                }
            }
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock=>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }
            foreach (var docType in TestData.docTypes)
            {
                _mockDocTypeRepo.Setup(mock=>
                    mock.GetById(docType.Id)).Returns(
                        TestData.docTypes.SingleOrDefault(v => v.Id == docType.Id));
            }

            _mockDocRepo.Setup(mock=>
                mock.Add(It.IsAny<Doc>())).Verifiable();
            _mockDocRepo.Setup(mock=>
                mock.Update(It.IsAny<Doc>())).Verifiable();
            _mockDocRepo.Setup(mock=>
                mock.Delete(It.IsAny<Doc>())).Verifiable();

            _service = new DocService(
                _mockDocRepo.Object,
                _mockMarkRepo.Object,
                _mockEmployeeRepo.Object,
                _mockDocTypeRepo.Object);
        }

        [Fact]
        public void GetAllSheetsByMarkId_ShouldReturnAllSheets()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int sheetDocTypeId = 1;

            // Act
            var sheets = _service.GetAllSheetsByMarkId(markId);

            // Assert
            Assert.Equal(TestData.docs.Where(
                v => v.Mark.Id == markId && v.Type.Id == sheetDocTypeId), sheets);
        }

        [Fact]
        public void GetAllAttachedByMarkId_ShouldReturnAllAttached()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int sheetDocTypeId = 1;

            // Act
            var attachedDocs = _service.GetAllAttachedByMarkId(markId);

            // Assert
            Assert.Equal(TestData.docs.Where(
                v => v.Mark.Id == markId && v.Type.Id != sheetDocTypeId), attachedDocs);
        }

        [Fact]
        public void Create_ShouldCreateDoc()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int docTypeId = _rnd.Next(1, TestData.docTypes.Count());
            int creatorId = _rnd.Next(1, TestData.employees.Count());
            int inspectorId = _rnd.Next(1, TestData.employees.Count());
            int normContrId = _rnd.Next(1, TestData.employees.Count());

            var newDoc = new Doc
            {
                Name="NewCreate",
                NumOfPages=1,
                Form=1.0f,
            };
            
            // Act
            _service.Create(newDoc, markId, docTypeId, creatorId, inspectorId, normContrId);

            // Assert
            _mockDocRepo.Verify(mock => mock.Add(It.IsAny<Doc>()), Times.Once);
            Assert.NotNull(newDoc.Mark);
            Assert.NotNull(newDoc.Type);
            Assert.NotNull(newDoc.Creator);
            Assert.NotNull(newDoc.Inspector);
            Assert.NotNull(newDoc.NormContr);
        }

        [Fact]
        public void Create_ShouldFailWithNull()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int wrongMarkId = 999;
            int docTypeId = _rnd.Next(1, TestData.docTypes.Count());
            int wrongDocTypeId = 999;
            int creatorId = _rnd.Next(1, TestData.employees.Count());
            int wrongCreatorId = 999;
            int inspectorId = _rnd.Next(1, TestData.employees.Count());
            int wrongInspectorId = 999;
            int normContrId = _rnd.Next(1, TestData.employees.Count());
            int wrongNormContrId = 999;

            var newDoc = new Doc
            {
                Name="NewCreate",
                NumOfPages=1,
                Form=1.0f,
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null, markId, docTypeId, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, wrongMarkId, docTypeId, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, wrongDocTypeId, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, wrongCreatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, creatorId, wrongInspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, creatorId, inspectorId, wrongNormContrId));

            _mockDocRepo.Verify(mock => mock.Add(It.IsAny<Doc>()), Times.Never);
        }

        // TBD!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // [Fact]
        // public void Create_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     // Possible conflict values
        //     var conflictMarkId = TestData.Docs[0].Mark.Id;
        //     var conflictDesignation = TestData.Docs[0].Designation;

        //     var newDoc = new Doc{
        //         Designation=conflictDesignation,
        //         Name="NewCreate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Create(newDoc, conflictMarkId));

        //     _mockDocRepo.Verify(mock => mock.Add(It.IsAny<Doc>()), Times.Never);
        // }
        
        [Fact]
        public void Update_ShouldUpdateDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.docs.Count());
            var newName = "NewUpdate";

            var newDocRequest = new DocUpdateRequest
            {
                Name=newName,
            };
            
            // Act
            _service.Update(id, newDocRequest);

            // Assert
            _mockDocRepo.Verify(mock => mock.Update(It.IsAny<Doc>()), Times.Once);
            Assert.Equal(newName, TestData.docs.SingleOrDefault(v => v.Id == id).Name);
        }

        [Fact]
        public void Update_ShouldFailWithNull()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.docs.Count());
            int wrongId = 999;

            var newDocRequest = new DocUpdateRequest
            {
                Name="NewUpdate",
            };
            
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(wrongId, newDocRequest));

            _mockDocRepo.Verify(mock => mock.Update(It.IsAny<Doc>()), Times.Never);
        }

        // [Fact]
        // public void Update_ShouldFailWithConflict()
        // {
        //     // Arrange
        //     // Possible conflict values
        //     var conflictMarkId = TestData.Docs[0].Mark.Id;
        //     var conflictDesignation = TestData.Docs[0].Designation;
        //     var id = TestData.Docs[3].Id;

        //     var newDocRequest = new DocUpdateRequest{
        //         Designation=conflictDesignation,
        //         Name="NewUpdate",
        //     };
            
        //     // Act & Assert
        //     Assert.Throws<ConflictException>(() => _service.Update(id, newDocRequest));

        //     _mockDocRepo.Verify(mock => mock.Update(It.IsAny<Doc>()), Times.Never);
        // }

        [Fact]
        public void Delete_ShouldDeleteDoc()
        {
            // Arrange
            int id = _rnd.Next(1, TestData.docs.Count());
            
            // Act
            _service.Delete(id);

            // Assert
            _mockDocRepo.Verify(mock => mock.Delete(It.IsAny<Doc>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull()
        {
            // Arrange
            var wrongId = 999;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(wrongId));

            _mockDocRepo.Verify(mock => mock.Delete(It.IsAny<Doc>()), Times.Never);
        }
    }
}
