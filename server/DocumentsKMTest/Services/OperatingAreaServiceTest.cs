using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class OperatingAreaServiceTest
    {
        private readonly IOperatingAreaService _service;

        public OperatingAreaServiceTest()
        {
            // Arrange
            var repository = new Mock<IOperatingAreaRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.operatingAreas);
            
            _service = new OperatingAreaService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOperatingAreas()
        {
            // Act
            var returnedOperatingAreas = _service.GetAll();

            // Assert
            Assert.Equal(TestData.operatingAreas,
                returnedOperatingAreas);
        }
    }
}

