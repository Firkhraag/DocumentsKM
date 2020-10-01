using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class DepartmentServiceTest
    {
        [Fact]
        public void GetAllActive_ShouldReturnAllActiveDepartments()
        {
            // Arrange
            var filteredDepartments = TestData.departments.FindAll(d => d.IsActive);
            var mockDepartmentRepo = new Mock<IDepartmentRepo>();
            mockDepartmentRepo.Setup(mock=>
                mock.GetAllActive()).Returns(filteredDepartments);
            var service = new DepartmentService(mockDepartmentRepo.Object);
            
            // Act
            var returnedDepartments = service.GetAllActive().ToList();

            // Assert
            Assert.Equal(filteredDepartments, returnedDepartments);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetByNumber_ShouldReturnDepartment(int number)
        {
            // Arrange
            var mockDepartmentRepo = new Mock<IDepartmentRepo>();
            mockDepartmentRepo.Setup(mock=>
                mock.GetByNumber(number)).Returns(TestData.departments[number]);
            var service = new DepartmentService(mockDepartmentRepo.Object);
            
            // Act
            var department = service.GetByNumber(number);

            // Assert
            Assert.Equal(TestData.departments[number], department);
        }
    }
}