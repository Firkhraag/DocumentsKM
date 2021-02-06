using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionTypeServiceTest
    {
        private readonly IConstructionTypeService _service;

        public ConstructionTypeServiceTest()
        {
            // Arrange
            var repository = new Mock<IConstructionTypeRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.constructionTypes);

            _service = new ConstructionTypeService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnConstructionTypes()
        {
            // Act
            var returnedConstructionTypes = _service.GetAll();

            // Assert
            Assert.Equal(TestData.constructionTypes,
                returnedConstructionTypes);
        }
    }
}
