using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class LinkedDocTypeServiceTest
    {
        private readonly ILinkedDocTypeService _service;

        public LinkedDocTypeServiceTest()
        {
            // Arrange
            var repository = new Mock<ILinkedDocTypeRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.linkedDocTypes);
            
            _service = new LinkedDocTypeService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnLinkedDocTypes()
        {
            // Act
            var returnedLinkedDocTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.linkedDocTypes,
                returnedLinkedDocTypes);
        }
    }
}

