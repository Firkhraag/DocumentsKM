using System;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class EmployeeFetched
    {
        public Int32 Id { get; set; }

        public string Fullname { get; set; }

        public DepartmentFetched Department { get; set; }

        public Position Post { get; set; }

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
                // TBD
                DepartmentId = 2,
                PositionId = Post.Id,

                IsActive = DismissedDate == null,
            };
        }
    }
}
