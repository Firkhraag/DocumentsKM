using System;
using System.Collections.Generic;
using System.Linq;
using Personnel.Data;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepo _repository;
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IPositionRepo _positionRepo;

        public EmployeeService(
            IEmployeeRepo employeeRepo,
            IDepartmentRepo departmentRepo,
            IPositionRepo positionRepo)
        {
            _repository = employeeRepo;
            _departmentRepo = departmentRepo;
            _positionRepo = positionRepo;
        }

        public IEnumerable<Employee> GetAllByDepartmentId(int departmentId)
        {
            return _repository.GetAllByDepartmentId(departmentId);
        }

        public void Create(
            Employee employee,
            int departmentId,
            int positionId)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            var foundDepartment = _departmentRepo.GetById(departmentId);
            if (foundDepartment == null)
                throw new ArgumentNullException(nameof(foundDepartment));
            employee.Department = foundDepartment;

            var foundPosition = _positionRepo.GetById(positionId);
            if (foundPosition == null)
                throw new ArgumentNullException(nameof(foundPosition));
            employee.Position = foundPosition;

            _repository.Add(employee);
        }

        public void Update(
            int id,
            EmployeeRequest employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            var foundEmployee = _repository.GetById(id);
            if (foundEmployee == null)
                throw new ArgumentNullException(nameof(foundEmployee));

            foundEmployee.ShortName = employee.ShortName;
            foundEmployee.FullName = employee.FullName;
            _repository.Update(foundEmployee);
        }

        public void Delete(int id)
        {
            var foundEmployee = _repository.GetById(id);
            if (foundEmployee == null)
                throw new ArgumentNullException(nameof(foundEmployee));
            _repository.Delete(foundEmployee);
        }
    }
}
