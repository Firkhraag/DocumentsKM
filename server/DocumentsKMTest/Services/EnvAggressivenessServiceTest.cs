using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EnvAggressivenessServiceTest
    {
        private readonly IEnvAggressivenessService _service;

        public EnvAggressivenessServiceTest()
        {
            // Arrange
            var repository = new Mock<IEnvAggressivenessRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.envAggressiveness);

            _service = new EnvAggressivenessService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnEnvAggressiveness()
        {
            // Act
            var returnedEnvAggressivenesss = _service.GetAll();

            // Assert
            Assert.Equal(TestData.envAggressiveness,
                returnedEnvAggressivenesss);
        }
    }
}
