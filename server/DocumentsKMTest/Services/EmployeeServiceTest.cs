using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Services;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EmployeeServiceTest
    {
        private readonly Mock<IEmployeeRepo> _mockEmployeeRepo = new Mock<IEmployeeRepo>();
        private readonly EmployeeService _service;
        private readonly Random _rnd = new Random();

        public EmployeeServiceTest()
        {
            // Arrange
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock=>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }
            foreach (var department in TestData.departments)
            {
                _mockEmployeeRepo.Setup(mock=>
                    mock.GetAllByDepartmentId(department.Id)).Returns(
                        TestData.employees.Where(v => v.Department.Id == department.Id));

                foreach (var position in TestData.positions)
                {
                    // Will only consider an array of 2 positions
                    for (var i = 0; i < TestData.positions.Count(); i++)
                    {
                        if (TestData.positions[i].Id == position.Id)
                            continue;

                        _mockEmployeeRepo.Setup(mock=>
                            mock.GetAllByDepartmentIdAndPositions(
                                department.Id, new int[2]{position.Id, TestData.positions[i].Id})).Returns(
                                    TestData.employees.Where(
                                        v => v.Department.Id == department.Id &&
                                        new int[2]{position.Id, TestData.positions[i].Id}.Contains(v.Position.Id)));
                    }
                }
            }

            _service = new EmployeeService(_mockEmployeeRepo.Object);
        }

        [Fact]
        public void GetAllByDepartmentId_ShouldReturnAllEmployeesWithDepartmentId()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            
            // Act
            var returnedEmployees = _service.GetAllByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId), returnedEmployees);
        }

        // TBD

        // [Fact]
        // public void GetMarkApprovalEmployeesByDepartmentId_ShouldReturnAllEmployeesForMarkApproval()
        // {
        //     // Arrange
        //     int departmentId = _rnd.Next(1, TestData.departments.Count());
        //     int[] approvalPosIds = {1, 2};
            
        //     // Act
        //     var returnedEmployees = _service.GetMarkApprovalEmployeesByDepartmentId(departmentId);

        //     // Assert
        //     Assert.Equal(TestData.employees.Where(
        //         v => v.Department.Id == departmentId && approvalPosIds.Contains(v.Position.Id)), returnedEmployees);
        // }
    }
}

















// using System.Collections.Generic;
// using DocumentsKM.Models;
// using DocumentsKM.Data;
// using System.Linq;
// using Serilog;

// namespace DocumentsKM.Services
// {
//     public class EmployeeService : IEmployeeService
//     {
//         private IEmployeeRepo _repository;

//         public EmployeeService(IEmployeeRepo EmployeeRepo)
//         {
//             _repository = EmployeeRepo;
//         }

//         public IEnumerable<Employee> GetByDepartmentId(int departmentId)
//         {
//             return _repository.GetAllByDepartmentId(departmentId);
//         }

//         public IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentId(int departmentId)
//         {
//             int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            
//             var employees = _repository.GetAllByDepartmentIdAndPositions(
//                 departmentId,
//                 approvalPosIds);
//             return employees;
//         }

//         public (Employee, IEnumerable<Employee>,IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentId(
//             int departmentId)
//         {
//             var departmentHeadPosId = 7;
//             var chiefSpecialistPosId = 9;
//             var groupLeaderPosId = 10;
//             var mainBuilderPosId = 4;

//             var departmentHeadArr = _repository.GetAllByDepartmentIdAndPosition(
//                 departmentId,
//                 departmentHeadPosId);
//             // У каждого отдела должен быть один руководитель
//             if (departmentHeadArr.Count() != 1)
//                 throw new ConflictException();
//             var departmentHead = departmentHeadArr.ToList()[0];
//             var chiefSpecialists = _repository.GetAllByDepartmentIdAndPosition(
//                 departmentId,
//                 chiefSpecialistPosId);
//             var groupLeaders = _repository.GetAllByDepartmentIdAndPosition(
//                 departmentId,
//                 groupLeaderPosId);
//             var mainBuilders = _repository.GetAllByDepartmentIdAndPosition(
//                 departmentId,
//                 mainBuilderPosId);
//             return (departmentHead, chiefSpecialists, groupLeaders, mainBuilders);
//         }
//     }
// }
