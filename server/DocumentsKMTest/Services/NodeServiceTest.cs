using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class NodeServiceTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllByProjectId_ShouldReturnAllNodesWithGivenId(int projectId)
        {
            // Arrange
            var filteredNodes = TestData.nodes.FindAll(n => n.Project.Id == projectId);
            var mockNodeRepo = new Mock<INodeRepo>();
            mockNodeRepo.Setup(mock=>
                mock.GetAllByProjectId(projectId)).Returns(filteredNodes);
            var service = new NodeService(mockNodeRepo.Object);
            
            // Act
            var returnedNodes = service.GetAllByProjectId(projectId).ToList();

            // Assert
            Assert.Equal(filteredNodes, returnedNodes);
        }

        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void GetById_ShouldReturnNode(int id)
        // {
        //     // Arrange
        //     var mockNodeRepo = new Mock<INodeRepo>();
        //     mockNodeRepo.Setup(mock=>
        //         mock.GetById(id)).Returns(_repoNodes[id]);
        //     var service = new NodeService(mockNodeRepo.Object);
            
        //     // Act
        //     var node = service.GetById(id);

        //     // Assert
        //     Assert.Equal(_repoNodes[id], node);
        // }
    }
}