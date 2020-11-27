using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EnvAggressivenessServiceTest
    {
        private readonly EnvAggressivenessService _service;

        public EnvAggressivenessServiceTest()
        {
            // Arrange
            var mockEnvAggressivenessRepo = new Mock<IEnvAggressivenessRepo>();

            mockEnvAggressivenessRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.envAggressiveness);
            
            _service = new EnvAggressivenessService(mockEnvAggressivenessRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEnvAggressiveness()
        {
            // Act
            var returnedEnvAggressivenesss = _service.GetAll();

            // Assert
            Assert.Equal(TestData.envAggressiveness,
                returnedEnvAggressivenesss);
        }
    }
}

