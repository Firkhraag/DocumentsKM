// using System;
// using System.Linq;
// using DocumentsKM.Data;
// using DocumentsKM.Services;
// using Moq;
// using Xunit;

// namespace DocumentsKM.Tests
// {
//     public class SubnodeServiceTest
//     {
//         private readonly ISubnodeService _service;
//         private readonly Random _rnd = new Random();

//         public SubnodeServiceTest()
//         {
//             // Arrange
//             var repository = new Mock<ISubnodeRepo>();
//             var mockNodeRepo = new Mock<INodeRepo>();

//             foreach (var node in TestData.nodes)
//             {
//                 repository.Setup(mock =>
//                     mock.GetAllByNodeId(node.Id)).Returns(
//                         TestData.subnodes.Where(v => v.Node.Id == node.Id));
//             }

//             _service = new SubnodeService(
//                 repository.Object,
//                 mockNodeRepo.Object);
//         }

//         [Fact]
//         public void GetAllByNodeId_ShouldReturnSubnodes()
//         {
//             // Arrange
//             int nodeId = _rnd.Next(1, TestData.nodes.Count());

//             // Act
//             var returnedSubnodes = _service.GetAllByNodeId(nodeId);

//             // Assert
//             Assert.Equal(TestData.subnodes.Where(v => v.Node.Id == nodeId),
//                 returnedSubnodes);
//         }
//     }
// }
