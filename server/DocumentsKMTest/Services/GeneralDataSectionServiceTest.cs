using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GeneralDataSectionServiceTest
    {
        private readonly IGeneralDataSectionService _service;

        public GeneralDataSectionServiceTest()
        {
            // Arrange
            var repository = new Mock<IGeneralDataSectionRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.generalDataSections);

            _service = new GeneralDataSectionService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnGeneralDataSections()
        {
            // Act
            var returnedGeneralDataSections = _service.GetAll();

            // Assert
            Assert.Equal(TestData.generalDataSections,
                returnedGeneralDataSections);
        }
    }
}
