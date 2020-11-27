using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PaintworkTypeServiceTest
    {
        private readonly PaintworkTypeService _service;

        public PaintworkTypeServiceTest()
        {
            // Arrange
            var mockPaintworkTypeRepo = new Mock<IPaintworkTypeRepo>();

            mockPaintworkTypeRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.paintworkTypes);
            
            _service = new PaintworkTypeService(mockPaintworkTypeRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllPaintworkTypes()
        {
            // Act
            var returnedPaintworkTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.paintworkTypes,
                returnedPaintworkTypes);
        }
    }
}

