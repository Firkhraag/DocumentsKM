using System;
using System.Collections.Generic;
using Personnel.Data;
using Personnel.Dtos;
using Personnel.Models;

namespace Personnel.Services
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _repository;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _repository = departmentRepo;
        }

        public IEnumerable<Department> GetAll()
        {
            return _repository.GetAll();
        }

        public void Create(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            _repository.Add(department);
        }

        public void Update(
            int id,
            DepartmentRequest department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));
            var foundDepartment = _repository.GetById(id);
            if (foundDepartment == null)
                throw new ArgumentNullException(nameof(foundDepartment));

            foundDepartment.ShortName = department.ShortName;
            foundDepartment.LongName = department.LongName;
            _repository.Update(foundDepartment);
        }

        public void Delete(int id)
        {
            var foundDepartment = _repository.GetById(id);
            if (foundDepartment == null)
                throw new ArgumentNullException(nameof(foundDepartment));
            _repository.Delete(foundDepartment);
        }
    }
}
