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
        private readonly IEmployeeService _service;
        private readonly Random _rnd = new Random();
        private readonly int[] _approvalPosIds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public EmployeeServiceTest()
        {
            // Arrange
            foreach (var employee in TestData.employees)
            {
                _mockEmployeeRepo.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }
            foreach (var department in TestData.departments)
            {
                _mockEmployeeRepo.Setup(mock =>
                    mock.GetAllByDepartmentId(department.Id)).Returns(
                        TestData.employees.Where(v => v.Department.Id == department.Id));

                _mockEmployeeRepo.Setup(mock =>
                    mock.GetAllByDepartmentIdAndPositions(
                        department.Id, _approvalPosIds)).Returns(
                            TestData.employees.Where(
                                v => v.Department.Id == department.Id &&
                                _approvalPosIds.Contains(v.Position.Id)));

                foreach (var position in TestData.positions)
                {
                    _mockEmployeeRepo.Setup(mock =>
                        mock.GetAllByDepartmentIdAndPosition(
                            department.Id, position.Id)).Returns(
                                TestData.employees.Where(
                                    v => v.Department.Id == department.Id &&
                                    v.Position.Id == position.Id));
                }
            }

            _service = new EmployeeService(_mockEmployeeRepo.Object);
        }

        [Fact]
        public void GetAllByDepartmentId_ShouldReturnEmployees()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());

            // Act
            var returnedEmployees = _service.GetAllByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId), returnedEmployees);
        }

        [Fact]
        public void GetAllByDepartmentId_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Arrange
            int wrongDepartmentId = 999;

            // Act
            var returnedEmployees = _service.GetAllByDepartmentId(wrongDepartmentId);

            // Assert
            Assert.Empty(returnedEmployees);
        }

        [Fact]
        public void GetMarkApprovalEmployeesByDepartmentId_ShouldReturnEmployees()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());

            // Act
            var returnedEmployees = _service.GetMarkApprovalEmployeesByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.Where(
                v => v.Department.Id == departmentId && _approvalPosIds.Contains(
                    v.Position.Id)), returnedEmployees);
        }

        [Fact]
        public void GetMarkApprovalEmployeesByDepartmentId_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Arrange
            int wrongDepartmentId = 999;

            // Act
            var returnedEmployees = _service.GetMarkApprovalEmployeesByDepartmentId(wrongDepartmentId);

            // Assert
            Assert.Empty(returnedEmployees);
        }

        [Fact]
        public void GetMarkMainEmployeesByDepartmentId_ShouldReturnEmployeesForMarkApproval()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var departmentHeadPosId = 7;
            var chiefSpecialistPosId = 9;
            var groupLeaderPosId = 10;
            var mainBuilderPosId = 4;

            // Act
            (var departmentHead, var chiefSpecialists, var groupLeaders, var mainBuilders) =
                _service.GetMarkMainEmployeesByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.SingleOrDefault(
                v => v.Position.Id == departmentHeadPosId && v.Department.Id == departmentId), departmentHead);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id == chiefSpecialistPosId && v.Department.Id == departmentId), chiefSpecialists);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id == groupLeaderPosId && v.Department.Id == departmentId), groupLeaders);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id == mainBuilderPosId && v.Department.Id == departmentId), mainBuilders);
        }
    }
}
