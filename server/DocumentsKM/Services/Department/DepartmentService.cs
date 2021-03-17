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
            var departments = _repository.GetAll();
            foreach (var department in departments)
            {
                if (!departmentsFetched.Select(v => v.Id).Contains(department.Code))
                    _repository.Delete(department);
            }
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


            // var employees = new List<Employee>{};
            // foreach (var id in employeeIds)
            // {
            //     var employee = _employeeRepo.GetById(id);
            //     if (employee == null)
            //         throw new ArgumentNullException(nameof(employee));
            //     employees.Add(employee);
            // }

            // var markApprovals = _repository.GetAllByMarkId(markId);
            // var currentEmployeeIds = new List<int>{};
            // foreach (var ma in markApprovals)
            // {
            //     if (!employeeIds.Contains(ma.Employee.Id))
            //         _repository.Delete(ma);
            //     currentEmployeeIds.Add(ma.Employee.Id);
            // }

            // foreach (var (id, i) in employeeIds.WithIndex())
            //     if (!currentEmployeeIds.Contains(id))
            //         _repository.Add(
            //             new MarkApproval
            //             {
            //                 Mark=foundMark,
            //                 Employee=employees[i],
            //             });

            Log.Information(departmentsFetched.Count().ToString());
        }
    }
}
