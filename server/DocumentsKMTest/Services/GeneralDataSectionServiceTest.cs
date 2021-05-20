using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class GeneralDataSectionServiceTest
    {
        private readonly IGeneralDataSectionService _service;
        private readonly Mock<IGeneralDataSectionRepo> _repository = new Mock<IGeneralDataSectionRepo>();
        private readonly Random _rnd = new Random();

        public GeneralDataSectionServiceTest()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            var mockGeneralDataPointRepo = new Mock<IGeneralDataPointRepo>();
            var mockMarkGeneralDataSectionRepo = new Mock<IMarkGeneralDataSectionRepo>();

            foreach (var user in TestData.users)
            {
                mockUserRepo.Setup(mock =>
                    mock.GetById(user.Id)).Returns(
                        TestData.users.SingleOrDefault(v => v.Id == user.Id));

                _repository.Setup(mock =>
                    mock.GetAll()).Returns(
                        TestData.generalDataSections);
            }

            _service = new GeneralDataSectionService(
                _repository.Object,
                mockGeneralDataPointRepo.Object,
                mockMarkGeneralDataSectionRepo.Object,
                mockUserRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnGeneralDataSections()
        {
            // Act
            var returnedGeneralDataSections = _service.GetAll();

            // Assert
            Assert.Equal(TestData.generalDataSections, returnedGeneralDataSections);
        }
    }
}
