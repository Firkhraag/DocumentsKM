using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class UserResponse
    {
        public Int16 Id { get; set; }
        public EmployeeDepartmentResponse Employee { get; set; }

        public UserResponse(short id, Employee employee)
        {
            Id = id;
            Employee = new EmployeeDepartmentResponse
            {
                Id = employee.Id,
                Fullname = employee.Fullname,
                Department = employee.Department,
            };
        }
        public UserResponse() {}
    }
}
