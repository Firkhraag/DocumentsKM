using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProfileClassServiceTest
    {
        private readonly IProfileClassService _service;

        public ProfileClassServiceTest()
        {
            // Arrange
            var repository = new Mock<IProfileClassRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.profileClasses);

            _service = new ProfileClassService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnProfileClasss()
        {
            // Act
            var returnedProfileClasss = _service.GetAll();

            // Assert
            Assert.Equal(TestData.profileClasses,
                returnedProfileClasss);
        }
    }
}
