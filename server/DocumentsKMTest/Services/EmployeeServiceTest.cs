using System.Collections.Generic;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Models;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EmployeeServiceTest
    {
        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // public void GetAllApprovalByDepartmentNumber_ShouldReturnAllEmployees(int depatmentNumber)
        // {
        //     // Arrange
        //     int[] posIds = {1, 2};
        //     var filteredEmployees = TestData.employees.FindAll(e => e.Department.Number == depatmentNumber);
        //     var mockEmployeeRepo = new Mock<IEmployeeRepo>();
        //     mockEmployeeRepo.Setup(mock =>
        //         mock.GetAllByDepartmentNumberAndPositions(depatmentNumber, posIds)).Returns(filteredEmployees);
        //     var service = new EmployeeService(mockEmployeeRepo.Object);
            
        //     // Act
        //     var returnedEmployees = service.GetMarkApprovalEmployeesByDepartmentNumber(depatmentNumber).ToList();

        //     // Assert
        //     Assert.Equal(filteredEmployees, returnedEmployees);
        // }

        // [Theory]
        // [InlineData(0)]
        // [InlineData(1)]
        // [InlineData(2)]
        // public void GetById_ShouldReturnEmployee(int id)
        // {
        //     // Arrange
        //     var mockEmployeeRepo = new Mock<IEmployeeRepo>();
        //     mockEmployeeRepo.Setup(mock=>
        //         mock.GetById(id)).Returns(TestData.employees[id]);
        //     var service = new EmployeeService(mockEmployeeRepo.Object);
            
        //     // Act
        //     var employee = service.GetById(id);

        //     // Assert
        //     Assert.Equal(TestData.employees[id], employee);
        // }
    }
}