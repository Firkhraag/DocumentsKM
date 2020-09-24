using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockDepartmentRepo : IDepartmentRepo
    {
        // Initial Departments list
        private readonly List<Department> _departments = new List<Department>
        {
            new Department
            {
                Number=0,
                Name="Test department1",
                Code="D1",
                IsActive=true,
            },
            new Department
            {
                Number=1,
                Name="Test department2",
                Code="D2",
                IsActive=true,
            },
            new Department
            {
                Number=2,
                Name="Test department3",
                Code="D3",
                IsActive=true,
            },
        };

        // GetDepartmentById returns the Department with a given id
        public Department GetDepartmentByNumber(ulong number)
        {
            try
            {
                var department = _departments[Convert.ToInt32(number)];
                return department;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public IEnumerable<Department> GetAllActiveDepartments()
        {
            var departmentsToReturn = new List<Department>();
            foreach (Department department in _departments)
            {
                if (department.IsActive)
                {
                    departmentsToReturn.Add(department);
                }
            }
            return departmentsToReturn;
        }
    }
}
