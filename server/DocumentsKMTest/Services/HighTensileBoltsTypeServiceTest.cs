using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class HighTensileBoltsTypeServiceTest
    {
        private readonly HighTensileBoltsTypeService _service;

        public HighTensileBoltsTypeServiceTest()
        {
            // Arrange
            var mockHighTensileBoltsTypeRepo = new Mock<IHighTensileBoltsTypeRepo>();

            mockHighTensileBoltsTypeRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.highTensileBoltsTypes);
            
            _service = new HighTensileBoltsTypeService(mockHighTensileBoltsTypeRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllHighTensileBoltsTypes()
        {
            // Act
            var returnedHighTensileBoltsTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes,
                returnedHighTensileBoltsTypes);
        }
    }
}

