using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SheetNameServiceTest
    {
        private readonly SheetNameService _service;

        public SheetNameServiceTest()
        {
            // Arrange
            var mockSheetNameRepo = new Mock<ISheetNameRepo>();

            mockSheetNameRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.sheetNames);
            
            _service = new SheetNameService(mockSheetNameRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllSheetNames()
        {
            // Act
            var returnedSheetNames = _service.GetAll();

            // Assert
            Assert.Equal(TestData.sheetNames,
                returnedSheetNames);
        }
    }
}

