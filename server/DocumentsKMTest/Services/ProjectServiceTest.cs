using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectServiceTest
    {
        private readonly IProjectService _service;

        public ProjectServiceTest()
        {
            // Arrange
            var repository = new Mock<IProjectRepo>();

            repository.Setup(mock=>
                mock.GetAll()).Returns(TestData.projects);
            
            _service = new ProjectService(repository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnProjects()
        {
            // Act
            var returnedProjects = _service.GetAll();

            // Assert
            Assert.Equal(TestData.projects,
                returnedProjects);
        }
    }
}
