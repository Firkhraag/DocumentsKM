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
            return _repository.GetAllActive();
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
                    var name = departmentFetched.Name.Trim();
                    if (foundDepartment.Name != name)
                    {
                        foundDepartment.Name = name;
                        wasChanged = true;
                    }
                    var shortName = departmentFetched.Reduction.Trim();
                    if (foundDepartment.ShortName != shortName)
                    {
                        foundDepartment.ShortName = shortName;
                        wasChanged = true;
                    }
                    if (foundDepartment.IsActive != departmentFetched.Enable)
                    {
                        foundDepartment.IsActive = departmentFetched.Enable;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundDepartment);
                }
            }
        }
    }
}
