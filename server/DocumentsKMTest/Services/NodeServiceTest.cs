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
        private readonly INodeService _service;
        private readonly Random _rnd = new Random();

        public NodeServiceTest()
        {
            // Arrange
            var repository = new Mock<INodeRepo>();

            foreach (var project in TestData.projects)
            {
                repository.Setup(mock =>
                    mock.GetAllByProjectId(project.Id)).Returns(
                        TestData.nodes.Where(v => v.ProjectId == project.Id));
            }

            foreach (var node in TestData.nodes)
            {
                repository.Setup(mock =>
                    mock.GetById(node.Id)).Returns(
                        TestData.nodes.SingleOrDefault(v => v.Id == node.Id));
            }

            _service = new NodeService(repository.Object);
        }

        [Fact]
        public void GetAllByProjectId_ShouldReturnNodes()
        {
            // Arrange
            int projectId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var returnedNodes = _service.GetAllByProjectId(projectId);

            // Assert
            Assert.Equal(TestData.nodes.Where(v => v.ProjectId == projectId), returnedNodes);
        }

        [Fact]
        public void GetAllByProjectId_ShouldNull_WhenWrongProjectId()
        {
            // Act
            var returnedNodes = _service.GetAllByProjectId(999);

            // Assert
            Assert.Empty(returnedNodes);
        }

        [Fact]
        public void GetById_ShouldReturnNode()
        {
            // Arrange
            int nodeId = _rnd.Next(1, TestData.nodes.Count());

            // Act
            var returnedNode = _service.GetById(nodeId);

            // Assert
            Assert.Equal(TestData.nodes.SingleOrDefault(
                v => v.Id == nodeId), returnedNode);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Act
            var returnedNode = _service.GetById(999);

            // Assert
            Assert.Null(returnedNode);
        }
    }
}
