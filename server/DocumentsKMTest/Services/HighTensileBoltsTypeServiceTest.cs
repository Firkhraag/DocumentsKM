using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class HighTensileBoltsTypeServiceTest
    {
        private readonly IHighTensileBoltsTypeService _service;

        public HighTensileBoltsTypeServiceTest()
        {
            // Arrange
            var repository = new Mock<IHighTensileBoltsTypeRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.highTensileBoltsTypes);

            _service = new HighTensileBoltsTypeService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnHighTensileBoltsTypes()
        {
            // Act
            var returnedHighTensileBoltsTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.highTensileBoltsTypes,
                returnedHighTensileBoltsTypes);
        }
    }
}
