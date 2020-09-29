using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockEmployeeRepo : IEmployeeRepo
    {
        private readonly List<Employee> _employees;

        public MockEmployeeRepo(IDepartmentRepo departmentRepo, IPositionRepo positionRepo)
        {
            // Начальные значения
            _employees = new List<Employee>
            {
                new Employee
                {
                    Id=0,
                    FullName="Test employee1",
                    Department=departmentRepo.GetByNumber(0),
                    Position=positionRepo.GetByCode(1100),
                },
                new Employee
                {
                    Id=1,
                    FullName="Test employee2",
                    Department=departmentRepo.GetByNumber(0),
                    Position=positionRepo.GetByCode(1285),
                },
                new Employee
                {
                    Id=2,
                    FullName="Test employee3",
                    Department=departmentRepo.GetByNumber(1),
                    Position=positionRepo.GetByCode(1185),
                },
            };
        }

        public Employee GetById(int id)
        {
            try
            {
                return _employees[id];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public IEnumerable<Employee> GetAllByDepartmentNumberWithPositions(int departmentNumber, int minPosCode, int maxPosCode)
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
            // Asc sort
            specialistsToReturn.Sort((x, y) => x.Position.Code.CompareTo(y.Position.Code));
            return specialistsToReturn;
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
