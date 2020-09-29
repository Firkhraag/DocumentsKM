using System;
using System.Collections.Generic;
using DocumentsKM.Models;

namespace DocumentsKM.Data
{
    // Мок репозитория для тестирования
    public class MockDepartmentRepo : IDepartmentRepo
    {
        // Начальные значения
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

        public IEnumerable<Department> GetAllActive()
        {
            var departmentsToReturn = new List<Department>();
            foreach (Department department in _departments)
            {
                if (department.IsActive)
                    departmentsToReturn.Add(department);
            }
            return departmentsToReturn;
        }

        public Department GetByNumber(int number)
        {
            try
            {
                // Считаем, что номер отдела это id
                return _departments[number];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}
