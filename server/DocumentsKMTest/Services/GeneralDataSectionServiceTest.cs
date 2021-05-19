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
        private readonly int _maxUserId = 3;

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
                    mock.GetAllByUserId(user.Id)).Returns(
                        TestData.generalDataSections.Where(
                            v => v.User.Id == user.Id));
            }

            _service = new GeneralDataSectionService(
                _repository.Object,
                mockGeneralDataPointRepo.Object,
                mockMarkGeneralDataSectionRepo.Object,
                mockUserRepo.Object);
        }

        [Fact]
        public void GetAllByUserId_ShouldReturnGeneralDataSections()
        {
            // Act
            int userId = _rnd.Next(1, _maxUserId);
            var returnedGeneralDataSections = _service.GetAllByUserId(userId);

            // Assert
            Assert.Equal(TestData.generalDataSections.Where(v => v.User.Id == userId),
                returnedGeneralDataSections);
        }
    }
}
