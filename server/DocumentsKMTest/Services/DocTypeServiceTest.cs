using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocTypeServiceTest
    {
        [Fact]
        public void GetAllAttachedDocTypes_ShouldReturnAllDocTypes()
        {
            // Arrange
            var docTypes = TestData.docTypes;
            var mockDocTypeRepo = new Mock<IDocTypeRepo>();
            mockDocTypeRepo.Setup(mock=>
                mock.GetAllExceptId(1)).Returns(docTypes.Where(v => v.Id != 1));
            mockDocTypeRepo.Setup(mock=>
                mock.GetAllExceptId(2)).Returns(docTypes.Where(v => v.Id != 2));
            mockDocTypeRepo.Setup(mock=>
                mock.GetAllExceptId(3)).Returns(docTypes.Where(v => v.Id != 3));
            var service = new DocTypeService(mockDocTypeRepo.Object);
            
            // Act
            var returnedDocTypes = service.GetAllAttached();

            // Assert
            Assert.Equal(docTypes.Where(v => v.Id != 1), returnedDocTypes);
        }
    }
}

