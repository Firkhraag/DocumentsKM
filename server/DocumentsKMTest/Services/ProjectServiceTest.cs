using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectServiceTest
    {
        private readonly IProjectService _service;
        private readonly Random _rnd = new Random();

        public ProjectServiceTest()
        {
            // Arrange
            var repository = new Mock<IProjectRepo>();

            repository.Setup(mock =>
                mock.GetAll()).Returns(TestData.projects);

            foreach (var project in TestData.projects)
            {
                repository.Setup(mock =>
                    mock.GetById(project.Id)).Returns(
                        TestData.projects.SingleOrDefault(v => v.Id == project.Id));
            }

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

        [Fact]
        public void GetById_ShouldReturnProject()
        {
            // Arrange
            int projectId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var returnedProject = _service.GetById(projectId);

            // Assert
            Assert.Equal(TestData.projects.SingleOrDefault(
                v => v.Id == projectId), returnedProject);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Act
            var returnedProject = _service.GetById(999);

            // Assert
            Assert.Null(returnedProject);
        }
    }
}
