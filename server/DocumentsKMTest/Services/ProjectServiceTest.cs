using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class ProjectServiceTest
    {
        // [Fact]
        // public void GetAll_ShouldReturnAllProjects()
        // {
        //     // Arrange
        //     var mockProjectRepo = new Mock<IProjectRepo>();
        //     mockProjectRepo.Setup(mock=>
        //         mock.GetAll()).Returns(TestData.projects);
        //     var service = new ProjectService(mockProjectRepo.Object);
            
        //     // Act
        //     var returnedProjects = service.GetAll().ToList();

        //     // Assert
        //     Assert.Equal(TestData.projects, returnedProjects);
        // }

        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void GetById_ShouldReturnProject(int id)
        // {
        //     // Arrange
        //     var mockProjectRepo = new Mock<IProjectRepo>();
        //     mockProjectRepo.Setup(mock=>
        //         mock.GetById(id)).Returns(TestData.projects[id]);
        //     var service = new ProjectService(mockProjectRepo.Object);
            
        //     // Act
        //     var project = service.GetById(id);

        //     // Assert
        //     Assert.Equal(TestData.projects[id], project);
        // }
    }
}