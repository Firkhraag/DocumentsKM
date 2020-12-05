using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionMaterialServiceTest
    {
        private readonly IConstructionMaterialService _service;

        public ConstructionMaterialServiceTest()
        {
            // Arrange
            var repository = new Mock<IConstructionMaterialRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.constructionMaterials);
            
            _service = new ConstructionMaterialService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnConstructionMaterials()
        {
            // Act
            var returnedConstructionMaterials = _service.GetAll();

            // Assert
            Assert.Equal(TestData.constructionMaterials,
                returnedConstructionMaterials);
        }
    }
}

