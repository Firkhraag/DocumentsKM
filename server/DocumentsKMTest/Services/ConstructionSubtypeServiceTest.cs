using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ConstructionSubtypeServiceTest
    {
        private readonly IConstructionSubtypeService _service;
        private readonly Random _rnd = new Random();

        public ConstructionSubtypeServiceTest()
        {
            // Arrange
            var repository = new Mock<IConstructionSubtypeRepo>();

            foreach (var type in TestData.constructionTypes)
            {
                repository.Setup(mock =>
                    mock.GetAllByTypeId(type.Id)).Returns(
                        TestData.constructionSubtypes.Where(v => v.Type.Id == type.Id));
            }

            _service = new ConstructionSubtypeService(repository.Object);
        }

        [Fact]
        public void GetAllByTypeId_ShouldReturnConstructionSubtypes()
        {
            // Arrange
            int typeId = _rnd.Next(1, TestData.constructionTypes.Count());

            // Act
            var returnedConstructionSubtypes = _service.GetAllByTypeId(typeId);

            // Assert
            Assert.Equal(TestData.constructionSubtypes.Where(v => v.Type.Id == typeId),
                returnedConstructionSubtypes);
        }
    }
}
