using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
using Serilog;
using System.Linq;

namespace DocumentsKM.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _repository;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _repository = departmentRepo;
        }

        public IEnumerable<Department> GetAll()
        {
            return _repository.GetAll();
        }

        public void UpdateAll(List<DepartmentFetched> departmentsFetched)
        {
            // Log.Information(departmentsFetched.ToList()[2].Reduction);
            Log.Information(departmentsFetched.Count().ToString());
        }
    }
}
