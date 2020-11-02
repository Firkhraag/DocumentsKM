using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class SubnodeServiceTest
    {
        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void GetAllByNodeId_ShouldReturnAllSubnodesWithGivenId(int nodeId)
        // {
        //     // Arrange
        //     var filteredSubnodes = TestData.subnodes.FindAll(s => s.Node.Id == nodeId);
        //     var mockSubnodeRepo = new Mock<ISubnodeRepo>();
        //     mockSubnodeRepo.Setup(mock=>
        //         mock.GetAllByNodeId(nodeId)).Returns(filteredSubnodes);
        //     var service = new SubnodeService(mockSubnodeRepo.Object);
            
        //     // Act
        //     var returnedSubnodes = service.GetAllByNodeId(nodeId).ToList();

        //     // Assert
        //     Assert.Equal(filteredSubnodes, returnedSubnodes);
        // }

        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void GetById_ShouldReturnSubnode(int id)
        // {
        //     // Arrange
        //     var mockSubnodeRepo = new Mock<ISubnodeRepo>();
        //     mockSubnodeRepo.Setup(mock=>
        //         mock.GetById(id)).Returns(_repoSubnodes[id]);
        //     var service = new SubnodeService(mockSubnodeRepo.Object);
            
        //     // Act
        //     var subnode = service.GetById(id);

        //     // Assert
        //     Assert.Equal(_repoSubnodes[id], subnode);
        // }
    }
}