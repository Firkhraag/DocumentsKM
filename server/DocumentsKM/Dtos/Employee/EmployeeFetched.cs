using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class EmployeeFetched
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public DepartmentFetched DepartmentFetched { get; set; }

        public Position Position { get; set; }
    }
}
