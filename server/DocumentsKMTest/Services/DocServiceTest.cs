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
    public class DocServiceTest
    {
        private readonly Mock<IDocRepo> _repository = new Mock<IDocRepo>();
        private readonly Mock<IMarkRepo> _mockMarkRepo = new Mock<IMarkRepo>();
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly Mock<IDocTypeRepo> _mockDocTypeRepo = new Mock<IDocTypeRepo>();
        private readonly IDocService _service;
        private readonly Random _rnd = new Random();
        private readonly List<Doc> _docs = new List<Doc> { };
        private readonly int _maxMarkId = 3;

        public DocServiceTest()
        {
            // Arrange
            foreach (var doc in TestData.docs)
            {
                _docs.Add(new Doc
                {
                    Id = doc.Id,
                    Mark = doc.Mark,
                    Num = doc.Num,
                    Type = doc.Type,
                    Name = doc.Name,
                    Form = doc.Form,
                    Creator = doc.Creator,
                    Inspector = doc.Inspector,
                    NormContr = doc.NormContr,
                    ReleaseNum = doc.ReleaseNum,
                    NumOfPages = doc.NumOfPages,
                    Note = doc.Note,
                });
            }
            foreach (var doc in _docs)
            {
                _repository.Setup(mock =>
                    mock.GetById(doc.Id)).Returns(
                        _docs.SingleOrDefault(v => v.Id == doc.Id));
            }
            foreach (var mark in TestData.marks)
            {
                _mockMarkRepo.Setup(mock =>
                    mock.GetById(mark.Id)).Returns(
                        TestData.marks.SingleOrDefault(v => v.Id == mark.Id));

                foreach (var docType in TestData.docTypes)
                {
                    _repository.Setup(mock =>
                        mock.GetAllByMarkIdAndDocType(mark.Id, docType.Id)).Returns(
                            _docs.Where(
                                v => v.Mark.Id == mark.Id && v.Type.Id == docType.Id));

                    _repository.Setup(mock =>
                        mock.GetAllByMarkIdAndNotDocType(mark.Id, docType.Id)).Returns(
                            _docs.Where(
                                v => v.Mark.Id == mark.Id && v.Type.Id != docType.Id));
                }
            }
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }
            foreach (var docType in TestData.docTypes)
            {
                _mockDocTypeRepo.Setup(mock =>
                    mock.GetById(docType.Id)).Returns(
                        TestData.docTypes.SingleOrDefault(v => v.Id == docType.Id));
            }

            _repository.Setup(mock =>
                mock.Add(It.IsAny<Doc>())).Verifiable();
            _repository.Setup(mock =>
                mock.Update(It.IsAny<Doc>())).Verifiable();
            _repository.Setup(mock =>
                mock.Delete(It.IsAny<Doc>())).Verifiable();

            _service = new DocService(
                _repository.Object,
                _mockMarkRepo.Object,
                _mockEmployeeRepo.Object,
                _mockDocTypeRepo.Object);
        }

        [Fact]
        public void GetAllSheetsByMarkId_ShouldReturnSheets()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            int sheetDocTypeId = 1;

            // Act
            var returnedSheets = _service.GetAllSheetsByMarkId(markId);

            // Assert
            Assert.Equal(_docs.Where(
                v => v.Mark.Id == markId && v.Type.Id == sheetDocTypeId), returnedSheets);
        }

        [Fact]
        public void GetAllSheetsByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Act
            var returnedSheets = _service.GetAllSheetsByMarkId(999);

            // Assert
            Assert.Empty(returnedSheets);
        }

        [Fact]
        public void GetAllAttachedByMarkId_ShouldReturnAttached()
        {
            // Arrange
            int markId = _rnd.Next(1, _maxMarkId);
            int sheetDocTypeId = 1;

            // Act
            var returnedAttachedDocs = _service.GetAllAttachedByMarkId(markId);

            // Assert
            Assert.Equal(_docs.Where(
                v => v.Mark.Id == markId && v.Type.Id != sheetDocTypeId), returnedAttachedDocs);
        }

        [Fact]
        public void GetAllAttachedByMarkId_ShouldReturnEmptyArray_WhenWrongMarkId()
        {
            // Act
            var returnedAttachedDocs = _service.GetAllSheetsByMarkId(999);

            // Assert
            Assert.Empty(returnedAttachedDocs);
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
                Name = "NewCreate",
                NumOfPages = 1,
                Form = 1.0f,
            };

            // Act
            _service.Create(
                newDoc, markId, docTypeId, creatorId, inspectorId, normContrId);

            // Assert
            _repository.Verify(mock => mock.Add(It.IsAny<Doc>()), Times.Once);
            Assert.NotNull(newDoc.Mark);
            Assert.NotNull(newDoc.Type);
            Assert.NotNull(newDoc.Creator);
            Assert.NotNull(newDoc.Inspector);
            Assert.NotNull(newDoc.NormContr);
        }

        [Fact]
        public void Create_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int markId = _rnd.Next(1, TestData.marks.Count());
            int docTypeId = _rnd.Next(1, TestData.docTypes.Count());
            int creatorId = _rnd.Next(1, TestData.employees.Count());
            int inspectorId = _rnd.Next(1, TestData.employees.Count());
            int normContrId = _rnd.Next(1, TestData.employees.Count());

            var newDoc = new Doc
            {
                Name = "NewCreate",
                NumOfPages = 1,
                Form = 1.0f,
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                null, markId, docTypeId, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, 999, docTypeId, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, 999, creatorId, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, 999, inspectorId, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, creatorId, 999, normContrId));
            Assert.Throws<ArgumentNullException>(() => _service.Create(
                newDoc, markId, docTypeId, creatorId, inspectorId, 999));

            _repository.Verify(mock => mock.Add(It.IsAny<Doc>()), Times.Never);
        }

        [Fact]
        public void Update_ShouldUpdateDoc()
        {
            // Arrange
            int id = _rnd.Next(2, _docs.Count());
            var newName = "NewUpdate";
            var newTypeId = _docs[0].Type.Id;
            var newNumOfPages = 9;
            var newForm = 9.0f;

            var newDocRequest = new DocUpdateRequest
            {
                Name = newName,
                TypeId = newTypeId,
                NumOfPages = newNumOfPages,
                Form = newForm,
            };

            // Act
            _service.Update(id, newDocRequest);

            // Assert
            _repository.Verify(mock => mock.Update(It.IsAny<Doc>()), Times.Once);
            Assert.Equal(newTypeId, _docs.SingleOrDefault(v => v.Id == id).Type.Id);
            Assert.Equal(newName, _docs.SingleOrDefault(v => v.Id == id).Name);
            Assert.Equal(newNumOfPages, _docs.SingleOrDefault(v => v.Id == id).NumOfPages);
            Assert.Equal(newForm, _docs.SingleOrDefault(v => v.Id == id).Form);
        }

        [Fact]
        public void Update_ShouldFailWithNull_WhenWrongValues()
        {
            // Arrange
            int id = _rnd.Next(1, _docs.Count());

            var newDocRequest = new DocUpdateRequest
            {
                Name = "NewUpdate",
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Update(id, null));
            Assert.Throws<ArgumentNullException>(() => _service.Update(
                999, newDocRequest));

            _repository.Verify(mock => mock.Update(It.IsAny<Doc>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldDeleteDoc()
        {
            // Arrange
            int id = _rnd.Next(1, _docs.Count());

            // Act
            _service.Delete(id);

            // Assert
            _repository.Verify(mock => mock.Delete(It.IsAny<Doc>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldFailWithNull_WhenWrongId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.Delete(999));

            _repository.Verify(mock => mock.Delete(It.IsAny<Doc>()), Times.Never);
        }
    }
}
