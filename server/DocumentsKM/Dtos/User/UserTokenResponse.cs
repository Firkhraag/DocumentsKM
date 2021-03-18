using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class UserTokenResponse
    {
        public Int16 Id { get; set; }
        public EmployeeDepartmentResponse Employee { get; set; }
        public string Token { get; set; }

        public UserTokenResponse(short id, Employee employee, string token)
        {
            Id = id;
            Employee = new EmployeeDepartmentResponse
            {
                Id = employee.Id,
                Fullname = employee.Fullname,
                Department = employee.Department,
            };
            Token = token;
        }
        public UserTokenResponse() {}
    }
}
