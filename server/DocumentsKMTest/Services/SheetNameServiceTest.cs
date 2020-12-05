using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SheetNameServiceTest
    {
        private readonly ISheetNameService _service;

        public SheetNameServiceTest()
        {
            // Arrange
            var repository = new Mock<ISheetNameRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.sheetNames);
            
            _service = new SheetNameService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnSheetNames()
        {
            // Act
            var returnedSheetNames = _service.GetAll();

            // Assert
            Assert.Equal(TestData.sheetNames,
                returnedSheetNames);
        }
    }
}

