using System.Collections.Generic;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAll();
        void Create(Department department);
        void Update(int id, DepartmentRequest department);
        void Delete(int id);
    }
}
