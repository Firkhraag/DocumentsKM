using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SubnodeServiceTest
    {
        private readonly ISubnodeService _service;
        private readonly Random _rnd = new Random();

        public SubnodeServiceTest()
        {
            // Arrange
            var repository = new Mock<ISubnodeRepo>();

            foreach (var project in TestData.projects)
            {
                repository.Setup(mock =>
                    mock.GetAllByNodeId(project.Id)).Returns(
                        TestData.subnodes.Where(v => v.NodeId == project.Id));
            }

            foreach (var subnode in TestData.subnodes)
            {
                repository.Setup(mock =>
                    mock.GetById(subnode.Id)).Returns(
                        TestData.subnodes.SingleOrDefault(v => v.Id == subnode.Id));
            }

            _service = new SubnodeService(repository.Object);
        }

        [Fact]
        public void GetAllByNodeId_ShouldReturnSubnodes()
        {
            // Arrange
            int nodeId = _rnd.Next(1, TestData.projects.Count());

            // Act
            var returnedSubnodes = _service.GetAllByNodeId(nodeId);

            // Assert
            Assert.Equal(TestData.subnodes.Where(v => v.NodeId == nodeId), returnedSubnodes);
        }

        [Fact]
        public void GetAllByNodeId_ShouldNull_WhenWrongnodeId()
        {
            // Act
            var returnedSubnodes = _service.GetAllByNodeId(999);

            // Assert
            Assert.Empty(returnedSubnodes);
        }

        [Fact]
        public void GetById_ShouldReturnSubnode()
        {
            // Arrange
            int subnodeId = _rnd.Next(1, TestData.subnodes.Count());

            // Act
            var returnedSubnode = _service.GetById(subnodeId);

            // Assert
            Assert.Equal(TestData.subnodes.SingleOrDefault(
                v => v.Id == subnodeId), returnedSubnode);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenWrongId()
        {
            // Act
            var returnedSubnode = _service.GetById(999);

            // Assert
            Assert.Null(returnedSubnode);
        }
    }
}
