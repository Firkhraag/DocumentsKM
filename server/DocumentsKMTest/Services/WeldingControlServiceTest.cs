using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class WeldingControlServiceTest
    {
        private readonly WeldingControlService _service;

        public WeldingControlServiceTest()
        {
            // Arrange
            var mockWeldingControlRepo = new Mock<IWeldingControlRepo>();

            mockWeldingControlRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.weldingControl);
            
            _service = new WeldingControlService(mockWeldingControlRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllWeldingControl()
        {
            // Act
            var returnedWeldingControls = _service.GetAll();

            // Assert
            Assert.Equal(TestData.weldingControl,
                returnedWeldingControls);
        }
    }
}

