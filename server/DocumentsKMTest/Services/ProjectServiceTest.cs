using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectServiceTest
    {
        private readonly ProjectService _service;

        public ProjectServiceTest()
        {
            // Arrange
            var mockProjectRepo = new Mock<IProjectRepo>();

            mockProjectRepo.Setup(mock=>
                mock.GetAll()).Returns(TestData.projects);
            
            _service = new ProjectService(mockProjectRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllProjects()
        {
            // Act
            var returnedProjects = _service.GetAll();

            // Assert
            Assert.Equal(TestData.projects,
                returnedProjects);
        }
    }
}
