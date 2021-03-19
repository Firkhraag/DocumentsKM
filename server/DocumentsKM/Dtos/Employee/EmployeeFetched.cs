using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class EmployeeFetched
    {
        public Int32 Id { get; set; }

        public string Fullname { get; set; }

        public DepartmentFetched DepartmentFetched { get; set; }

        public Position Position { get; set; }

        public DateTime? DismissedDate { get; set; }

        public Employee ToEmployee()
        {
            var split = Fullname.Split(" ");
            string name;
            try
            {
                name = split[0];
            }
            catch (IndexOutOfRangeException)
            {
                name = "";
            }
            return new Employee
            {
                Id = Id,
                Fullname = Fullname,
                Name = name,
                Department = DepartmentFetched.ToDepartment(),
                Position = Position,
                IsActive = DismissedDate == null,
            };
        }
    }
}
