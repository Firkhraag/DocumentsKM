using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class OperatingAreaServiceTest
    {
        private readonly OperatingAreaService _service;

        public OperatingAreaServiceTest()
        {
            // Arrange
            var mockOperatingAreaRepo = new Mock<IOperatingAreaRepo>();

            mockOperatingAreaRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.operatingAreas);
            
            _service = new OperatingAreaService(mockOperatingAreaRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllOperatingAreas()
        {
            // Act
            var returnedOperatingAreas = _service.GetAll();

            // Assert
            Assert.Equal(TestData.operatingAreas,
                returnedOperatingAreas);
        }
    }
}

