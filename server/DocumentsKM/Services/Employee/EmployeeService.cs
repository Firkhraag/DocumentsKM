using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;
using DocumentsKM.Dtos;

namespace DocumentsKM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _repository;

        public EmployeeService(IEmployeeRepo employeeRepo)
        {
            _repository = employeeRepo;
        }

        public IEnumerable<Employee> GetAllByDepartmentId(int departmentId)
        {
            return _repository.GetAllByDepartmentId(departmentId);
        }

        public IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentId(int departmentId)
        {
            int[] approvalPosIds = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            
            var employees = _repository.GetAllByDepartmentIdAndPositions(
                departmentId,
                approvalPosIds);
            return employees;
        }

        public (Employee, IEnumerable<Employee>,IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentId(
            int departmentId)
        {
            var departmentHeadPosId = 7;
            var chiefSpecialistPosId = 9;
            var groupLeaderPosId = 10;
            var mainBuilderPosId = 4;

            var departmentHeadArr = _repository.GetAllByDepartmentIdAndPosition(
                departmentId,
                departmentHeadPosId);
            var departmentHead = departmentHeadArr.ToList()[0];
            var chiefSpecialists = _repository.GetAllByDepartmentIdAndPosition(
                departmentId,
                chiefSpecialistPosId);
            var groupLeaders = _repository.GetAllByDepartmentIdAndPosition(
                departmentId,
                groupLeaderPosId);
            var mainBuilders = _repository.GetAllByDepartmentIdAndPosition(
                departmentId,
                mainBuilderPosId);
            return (departmentHead, chiefSpecialists, groupLeaders, mainBuilders);
        }

        public void UpdateAll(List<EmployeeFetched> employeesFetched)
        {
            var employees = _repository.GetAll();
            foreach (var employee in employees)
            {
                if (!employeesFetched.Select(v => v.Id).Contains(employee.Id))
                    _repository.Delete(employee);
            }
            foreach (var employeeFetched in employeesFetched)
            {
                var foundEmployee = employees.SingleOrDefault(v => v.Id == employeeFetched.Id);
                // if (foundEmployee == null)
                //     _repository.Add(employeeFetched.ToEmployee());
                // else
                // {
                //     var wasChanged = false;
                //     if (foundEmployee.Name != employeeFetched.Name)
                //     {
                //         foundEmployee.Name = employeeFetched.Name;
                //         wasChanged = true;
                //     }
                //     if (foundEmployee.ShortName != employeeFetched.Reduction)
                //     {
                //         foundEmployee.ShortName = employeeFetched.Reduction;
                //         wasChanged = true;
                //     }
                //     if (wasChanged)
                //         _repository.Update(foundEmployee);
                // }
            }
        }
    }
}
