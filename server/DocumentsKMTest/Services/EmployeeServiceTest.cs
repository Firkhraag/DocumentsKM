using System;
using System.Linq;
using DocumentsKM.Data;
using DocumentsKM.Helpers;
using DocumentsKM.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DocumentsKM.Tests
{
    public class EmployeeServiceTest
    {
        private readonly Mock<IEmployeeRepo> _repository = new Mock<IEmployeeRepo>();
        private readonly IEmployeeService _service;
        private readonly Random _rnd = new Random();

        public EmployeeServiceTest()
        {
            // Arrange
            foreach (var employee in TestData.employees)
            {
                _repository.Setup(mock =>
                    mock.GetById(employee.Id)).Returns(
                        TestData.employees.SingleOrDefault(v => v.Id == employee.Id));
            }
            foreach (var department in TestData.departments)
            {
                _repository.Setup(mock =>
                    mock.GetAllByDepartmentId(department.Id)).Returns(
                        TestData.employees.Where(v => v.Department.Id == department.Id && v.IsActive));

                _repository.Setup(mock =>
                    mock.GetAllByDepartmentIdAndPositions(
                        department.Id, 1, 10)).Returns(
                            TestData.employees.Where(
                                v => v.Department.Id == department.Id &&
                                v.Position.Id >= 1 && v.Position.Id <= 10 && v.IsActive));

                foreach (var position in TestData.positions)
                {
                    _repository.Setup(mock =>
                        mock.GetAllByDepartmentIdAndPosition(
                            department.Id, position.Id)).Returns(
                                TestData.employees.Where(
                                    v => v.Department.Id == department.Id &&
                                    v.Position.Id == position.Id && v.IsActive));

                    _repository.Setup(mock =>
                        mock.GetByDepartmentIdAndPosition(
                            department.Id, position.Id)).Returns(
                                TestData.employees.FirstOrDefault(
                                    v => v.Department.Id == department.Id &&
                                    v.Position.Id == position.Id && v.IsActive));

                    _repository.Setup(mock =>
                        mock.GetAllByDepartmentIdAndPositions(
                            department.Id, new int[] {position.Id, position.Id})).Returns(
                                TestData.employees.Where(
                                    v => v.Department.Id == department.Id &&
                                    v.Position.Id == position.Id && v.IsActive));

                    _repository.Setup(mock =>
                        mock.GetAllByDepartmentIdAndPositions(
                            department.Id, new int[] {position.Id, position.Id, position.Id, position.Id})).Returns(
                                TestData.employees.Where(
                                    v => v.Department.Id == department.Id &&
                                    v.Position.Id == position.Id && v.IsActive));
                }
            }

            IOptions<AppSettings> options = Options.Create<AppSettings>(new AppSettings()
            {
                DepartmentHeadPosId = 7,
                ActingDepartmentHeadPosId = 7,
                DeputyDepartmentHeadPosId = 7,
                ActingDeputyDepartmentHeadPosId = 7,

                ChiefSpecialistPosId = 9,
                ActingChiefSpecialistPosId = 9,

                GroupLeaderPosId = 10,
                ActingGroupLeaderPosId = 10,

                MainBuilderPosId = 4,

                ApprovalMinPosId = 1,
                ApprovalMaxPosId = 10,

                NormContrMinPosId = 1,
                NormContrMaxPosId = 10,
            });
            _service = new EmployeeService(_repository.Object, options);
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
                v => v.Department.Id == departmentId && v.IsActive), returnedEmployees);
        }

        [Fact]
        public void GetAllByDepartmentId_ShouldReturnEmptyArray_WhenWrongDepartmentId()
        {
            // Act
            var returnedEmployees = _service.GetAllByDepartmentId(999);

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
                v => v.Department.Id == departmentId &&
                v.Position.Id >= 1 &&
                v.Position.Id <= 10 &&
                v.IsActive), returnedEmployees);
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
        public void GetMarkMainEmployeesByDepartmentId_ShouldReturnEmployees()
        {
            // Arrange
            int departmentId = _rnd.Next(1, TestData.departments.Count());
            var departmentHeadPosId = 7;
            var chiefSpecialistPosId = 9;
            var groupLeaderPosId = 10;
            var minNormContrPos = 1;
            var maxNormContrPos = 10;

            // Act
            (var departmentHead, var chiefSpecialists, var groupLeaders, var normContrs) =
                _service.GetMarkMainEmployeesByDepartmentId(departmentId);

            // Assert
            Assert.Equal(TestData.employees.FirstOrDefault(
                v => v.Position.Id == departmentHeadPosId &&
                v.Department.Id == departmentId &&
                v.IsActive), departmentHead);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id == chiefSpecialistPosId &&
                v.Department.Id == departmentId &&
                v.IsActive), chiefSpecialists);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id == groupLeaderPosId &&
                v.Department.Id == departmentId &&
                v.IsActive), groupLeaders);
            Assert.Equal(TestData.employees.Where(
                v => v.Position.Id >= minNormContrPos &&
                v.Position.Id <= maxNormContrPos &&
                v.Department.Id == departmentId &&
                v.IsActive), normContrs);
        }
    }
}
