
using Personnel.Models;

namespace Personnel.Dtos
{
    public class DepartmentRabbitResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DepartmentRabbitResponse(Department department)
        {
            Id = department.Id;
            Name = department.ShortName;
        }
    }
}
