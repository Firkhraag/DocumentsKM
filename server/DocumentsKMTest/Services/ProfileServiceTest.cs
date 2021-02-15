using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProfileServiceTest
    {
        private readonly IProfileService _service;
        private readonly Random _rnd = new Random();

        public ProfileServiceTest()
        {
            // Arrange
            var repository = new Mock<IProfileRepo>();

            foreach (var profileClass in TestData.projects)
            {
                repository.Setup(mock =>
                    mock.GetAllByProfileClassId(profileClass.Id)).Returns(
                        TestData.profiles.Where(v => v.Class.Id == profileClass.Id));
            }

            _service = new ProfileService(repository.Object);
        }

        [Fact]
        public void GetAllByProfileClassId_ShouldReturnProfiles()
        {
            // Arrange
            int profileClassId = _rnd.Next(1, TestData.profileClasses.Count());

            // Act
            var returnedProfiles = _service.GetAllByProfileClassId(profileClassId);

            // Assert
            Assert.Equal(TestData.profiles.Where(v => v.Class.Id == profileClassId),
                returnedProfiles);
        }
    }
}