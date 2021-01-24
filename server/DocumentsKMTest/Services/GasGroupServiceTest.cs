using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GasGroupServiceTest
    {
        private readonly IGasGroupService _service;

        public GasGroupServiceTest()
        {
            // Arrange
            var repository = new Mock<IGasGroupRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.gasGroups);

            _service = new GasGroupService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnGasGroups()
        {
            // Act
            var returnedGasGroups = _service.GetAll();

            // Assert
            Assert.Equal(TestData.gasGroups,
                returnedGasGroups);
        }
    }
}
