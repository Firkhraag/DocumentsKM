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
    }
}