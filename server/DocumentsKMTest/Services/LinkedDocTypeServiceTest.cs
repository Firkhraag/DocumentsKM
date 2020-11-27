using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class LinkedDocTypeServiceTest
    {
        private readonly LinkedDocTypeService _service;

        public LinkedDocTypeServiceTest()
        {
            // Arrange
            var mockLinkedDocTypeRepo = new Mock<ILinkedDocTypeRepo>();

            mockLinkedDocTypeRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.linkedDocTypes);
            
            _service = new LinkedDocTypeService(mockLinkedDocTypeRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllLinkedDocTypes()
        {
            // Act
            var returnedLinkedDocTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.linkedDocTypes,
                returnedLinkedDocTypes);
        }
    }
}

