using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocTypeServiceTest
    {
        private readonly DocTypeService _service;

        public DocTypeServiceTest()
        {
            // Arrange
            var mockDocTypeRepo = new Mock<IDocTypeRepo>();

            foreach (var docType in TestData.docTypes)
            {
                mockDocTypeRepo.Setup(mock=>
                    mock.GetAllExceptId(docType.Id)).Returns(
                        TestData.docTypes.Where(v => v.Id != docType.Id));
            }

            _service = new DocTypeService(mockDocTypeRepo.Object);
        }

        [Fact]
        public void GetAllAttached_ShouldReturnAllDocTypes()
        {
            // Arrange
            int sheetDocTypeId = 1;

            // Act
            var returnedDocTypes = _service.GetAllAttached();

            // Assert
            Assert.Equal(TestData.docTypes.Where(v => v.Id != sheetDocTypeId), returnedDocTypes);
        }
    }
}

