using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class NodeServiceTest
    {
        private readonly NodeService _service;
        private readonly Random _rnd = new Random();

        public NodeServiceTest()
        {
            // Arrange
            var mockNodeRepo = new Mock<INodeRepo>();

            foreach (var project in TestData.projects)
            {
                mockNodeRepo.Setup(mock=>
                    mock.GetAllByProjectId(project.Id)).Returns(
                        TestData.nodes.Where(v => v.Project.Id == project.Id));
            }

            _service = new NodeService(mockNodeRepo.Object);
        }

        [Fact]
        public void GetAllByProjectId_ShouldReturnAllNodesWithGivenProjectId()
        {
            // Arrange
            int projectId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var returnedNodes = _service.GetAllByProjectId(projectId);

            // Assert
            Assert.Equal(TestData.nodes.Where(v => v.Project.Id == projectId),
                returnedNodes);
        }
    }
}
