using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockEmployeeRepo : IEmployeeRepo
    {
        // Initial Employees list
        private readonly List<Employee> _employees;

        public MockEmployeeRepo(IDepartmentRepo departmentRepo, IPositionRepo positionRepo)
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    Id=0,
                    FullName="Test employee1",
                    Department=departmentRepo.GetDepartmentByNumber(0),
                    Position=positionRepo.GetPositionByCode(1100),
                },
                new Employee
                {
                    Id=1,
                    FullName="Test employee2",
                    Department=departmentRepo.GetDepartmentByNumber(0),
                    Position=positionRepo.GetPositionByCode(1285),
                },
                new Employee
                {
                    Id=2,
                    FullName="Test employee3",
                    Department=departmentRepo.GetDepartmentByNumber(1),
                    Position=positionRepo.GetPositionByCode(1185),
                },
            };
        }

        // GetEmployeeById returns the Employee with a given id
        public Employee GetEmployeeById(ulong id)
        {
            try
            {
                var employee = _employees[Convert.ToInt32(id)];
                return employee;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public IEnumerable<Employee> GetAllApprovalSpecialists(ulong departmentNumber, uint minPosCode, uint maxPosCode)
        {
            var specialistsToReturn = new List<Employee>();
            foreach (Employee employee in _employees)
            {
                if (
                    (employee.Department.Number == departmentNumber) &&
                    (employee.Position.Code >= minPosCode) &&
                    (employee.Position.Code <= maxPosCode) &&
                    (employee.FiredDate == null)
                )
                {
                    specialistsToReturn.Add(employee);
                }
            }
            if (specialistsToReturn.Count == 0)
            {
                return null;
            }
            // Asc sort
            specialistsToReturn.Sort((x, y) => x.Position.Code.CompareTo(y.Position.Code));
            return specialistsToReturn;
        }
    }
}
