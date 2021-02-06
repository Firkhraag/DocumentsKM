using System.Collections.Generic;
using Personnel.Models;

namespace Personnel.Data
{
    public interface IDepartmentRepo
    {
        IEnumerable<Department> GetAll();
        Department GetById(int id);
        void Add(Department department);
        void Update(Department department);
        void Delete(Department department);
    }
}
