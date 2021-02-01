using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SteelServiceTest
    {
        private readonly ISteelService _service;

        public SteelServiceTest()
        {
            // Arrange
            var repository = new Mock<ISteelRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.steel);

            _service = new SteelService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnSteel()
        {
            // Act
            var returnedSteels = _service.GetAll();

            // Assert
            Assert.Equal(TestData.steel,
                returnedSteels);
        }
    }
}
