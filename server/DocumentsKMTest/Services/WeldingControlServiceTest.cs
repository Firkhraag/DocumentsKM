using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class WeldingControlServiceTest
    {
        private readonly IWeldingControlService _service;

        public WeldingControlServiceTest()
        {
            // Arrange
            var repository = new Mock<IWeldingControlRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.weldingControl);

            _service = new WeldingControlService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnWeldingControl()
        {
            // Act
            var returnedWeldingControls = _service.GetAll();

            // Assert
            Assert.Equal(TestData.weldingControl,
                returnedWeldingControls);
        }
    }
}
