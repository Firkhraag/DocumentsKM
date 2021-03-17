using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class DepartmentFetched
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Reduction { get; set; }

        public bool Enable { get; set; }

        public Department ToDepartment()
        {
            return new Department
            {
                Code = Id,
                Name = Name,
                ShortName = Reduction,
            };
        }
    }
}
