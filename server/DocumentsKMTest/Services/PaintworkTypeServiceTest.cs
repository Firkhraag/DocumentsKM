using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class PaintworkTypeServiceTest
    {
        private readonly IPaintworkTypeService _service;

        public PaintworkTypeServiceTest()
        {
            // Arrange
            var repository = new Mock<IPaintworkTypeRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.paintworkTypes);
            
            _service = new PaintworkTypeService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnPaintworkTypes()
        {
            // Act
            var returnedPaintworkTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.paintworkTypes,
                returnedPaintworkTypes);
        }
    }
}

