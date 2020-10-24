using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _repository;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _repository = departmentRepo;
        }

        public IEnumerable<Department> GetAllActive()
        {
            return _repository.GetAllActive();
        }
    }
}
