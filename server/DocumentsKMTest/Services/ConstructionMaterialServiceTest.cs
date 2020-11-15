using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionMaterialServiceTest
    {
        [Fact]
        public void GetAll_ShouldReturnAllConstructionMaterials()
        {
            // Arrange
            var constructionMaterials = TestData.constructionMaterials;
            var mockConstructionMaterialRepo = new Mock<IConstructionMaterialRepo>();
            mockConstructionMaterialRepo.Setup(mock=>
                mock.GetAll()).Returns(constructionMaterials);
            var service = new ConstructionMaterialService(mockConstructionMaterialRepo.Object);
            
            // Act
            var returnedConstructionMaterials = service.GetAll();

            // Assert
            Assert.Equal(constructionMaterials, returnedConstructionMaterials);
        }
    }
}

