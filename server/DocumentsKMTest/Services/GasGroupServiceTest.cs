using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GasGroupServiceTest
    {
        private readonly GasGroupService _service;

        public GasGroupServiceTest()
        {
            // Arrange
            var mockGasGroupRepo = new Mock<IGasGroupRepo>();

            mockGasGroupRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.gasGroups);
            
            _service = new GasGroupService(mockGasGroupRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllGasGroups()
        {
            // Act
            var returnedGasGroups = _service.GetAll();

            // Assert
            Assert.Equal(TestData.gasGroups,
                returnedGasGroups);
        }
    }
}

