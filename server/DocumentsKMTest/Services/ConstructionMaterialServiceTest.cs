using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionMaterialServiceTest
    {
        private readonly ConstructionMaterialService _service;

        public ConstructionMaterialServiceTest()
        {
            // Arrange
            var mockConstructionMaterialRepo = new Mock<IConstructionMaterialRepo>();

            mockConstructionMaterialRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.constructionMaterials);
            
            _service = new ConstructionMaterialService(mockConstructionMaterialRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllConstructionMaterials()
        {
            // Act
            var returnedConstructionMaterials = _service.GetAll();

            // Assert
            Assert.Equal(TestData.constructionMaterials,
                returnedConstructionMaterials);
        }
    }
}

