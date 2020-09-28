using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;

namespace DocumentsKM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepo _repository;

        public EmployeeService(IEmployeeRepo EmployeeRepo)
        {
            _repository = EmployeeRepo;
        }

        public IEnumerable<Employee> GetAllApprovalByDepartmentId(int departmentId)
        {
            int minPosCode = 1170;
            int maxPosCode = 1251;
            var employees = _repository.GetAllByDepartmentNumberWithPositions(departmentId, minPosCode, maxPosCode);
            return employees;
        }
    }
}
