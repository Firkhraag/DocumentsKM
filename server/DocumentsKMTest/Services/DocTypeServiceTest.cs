using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Helpers;
using DocumentsKM.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DocTypeServiceTest
    {
        private readonly IDocTypeService _service;

        public DocTypeServiceTest()
        {
            // Arrange
            var repository = new Mock<IDocTypeRepo>();

            foreach (var docType in TestData.docTypes)
            {
                repository.Setup(mock =>
                    mock.GetAllExceptId(docType.Id)).Returns(
                        TestData.docTypes.Where(v => v.Id != docType.Id));
            }

            IOptions<AppSettings> options = Options.Create<AppSettings>(new AppSettings()
            {
                SheetDocTypeId = 1,
            });

            _service = new DocTypeService(repository.Object, options);
        }

        [Fact]
        public void GetAllAttached_ShouldReturnDocTypes()
        {
            // Arrange
            int sheetDocTypeId = 1;

            // Act
            var returnedDocTypes = _service.GetAllAttached();

            // Assert
            Assert.Equal(TestData.docTypes.Where(
                v => v.Id != sheetDocTypeId), returnedDocTypes);
        }
    }
}
