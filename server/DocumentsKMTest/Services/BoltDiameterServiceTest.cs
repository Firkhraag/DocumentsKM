using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class BoltDiameterServiceTest
    {
        private readonly IBoltDiameterService _service;

        public BoltDiameterServiceTest()
        {
            // Arrange
            var repository = new Mock<IBoltDiameterRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.boltDiameters);

            _service = new BoltDiameterService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnBoltDiameters()
        {
            // Act
            var returnedBoltDiameters = _service.GetAll();

            // Assert
            Assert.Equal(TestData.boltDiameters,
                returnedBoltDiameters);
        }
    }
}
