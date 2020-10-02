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

        public IEnumerable<Employee> GetAllApprovalByDepartmentNumber(int departmentNumber)
        {
            int minPosCode = 1170;
            int maxPosCode = 1251;
            var employees = _repository.GetAllByDepartmentNumberWithPositions(
                departmentNumber,
                minPosCode,
                maxPosCode);
            return employees;
        }

        public Employee GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
