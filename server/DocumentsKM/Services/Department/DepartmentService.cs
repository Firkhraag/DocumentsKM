using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using DocumentsKM.Dtos;
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
            var departments = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var department in departments)
            // {
            //     if (!departmentsFetched.Select(v => v.Id).Contains(department.Code))
            //         _repository.Delete(department);
            // }
            foreach (var departmentFetched in departmentsFetched)
            {
                var foundDepartment = departments.SingleOrDefault(v => v.Code == departmentFetched.Id);
                if (foundDepartment == null)
                    _repository.Add(departmentFetched.ToDepartment());
                else
                {
                    var wasChanged = false;
                    if (foundDepartment.Name != departmentFetched.Name)
                    {
                        foundDepartment.Name = departmentFetched.Name;
                        wasChanged = true;
                    }
                    if (foundDepartment.ShortName != departmentFetched.Reduction)
                    {
                        foundDepartment.ShortName = departmentFetched.Reduction;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundDepartment);
                }
            }
        }
    }
}
