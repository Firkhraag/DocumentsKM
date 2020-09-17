using System;
using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    // Repo implementation for testing purposes
    public class MockEmployeeRepo : IEmployeeRepo
    {
        // Initial Employees list
        private List<Employee> _employees = new List<Employee>{
            new Employee{
                Id=0,
                FullName="Test1"
            },
            new Employee{
                Id=1,
                FullName="Test2"
            },
            new Employee{
                Id=2,
                FullName="Test3"
            }
        };

        // GetEmployeeById returns the Employee with a given id
        public Employee GetEmployeeById(ulong id)
        {
            try {
                var employee = _employees[Convert.ToInt32(id)];
                return employee;
            } catch (ArgumentOutOfRangeException) {
                return null;
            }
        }
    }
}
