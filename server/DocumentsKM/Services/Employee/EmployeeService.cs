using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;
using DocumentsKM.Dtos;
using Microsoft.Extensions.Options;
using DocumentsKM.Helpers;

namespace DocumentsKM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _repository;
        private readonly AppSettings _appSettings;

        public EmployeeService(
            IEmployeeRepo employeeRepo,
            IOptions<AppSettings> appSettings)
        {
            _repository = employeeRepo;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<Employee> GetAllByDepartmentId(int departmentId)
        {
            return _repository.GetAllByDepartmentId(departmentId);
        }

        public IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentId(int departmentId)
        {
            var employees = _repository.GetAllByDepartmentIdAndPositions(
                departmentId,
                _appSettings.ApprovalMinPosId,
                _appSettings.ApprovalMaxPosId);
            return employees;
        }

        public (Employee, IEnumerable<Employee>,IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentId(
            int departmentId)
        {
            var departmentHead = _repository.GetByDepartmentIdAndPosition(
                departmentId, _appSettings.DepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _repository.GetByDepartmentIdAndPosition(
                departmentId, _appSettings.ActingDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _repository.GetByDepartmentIdAndPosition(
                departmentId, _appSettings.DeputyDepartmentHeadPosId);
            if (departmentHead == null)
                departmentHead = _repository.GetByDepartmentIdAndPosition(
                departmentId, _appSettings.ActingDeputyDepartmentHeadPosId);

            var chiefSpecialists = _repository.GetAllByDepartmentIdAndPositions(
                departmentId,
                new int[] {_appSettings.ChiefSpecialistPosId, _appSettings.ActingChiefSpecialistPosId});
            var groupLeaders = _repository.GetAllByDepartmentIdAndPositions(
                departmentId,
                new int[] {_appSettings.GroupLeaderPosId, _appSettings.ActingGroupLeaderPosId});
            var normContrs = _repository.GetAllByDepartmentIdAndPositions(
                departmentId,
                _appSettings.ApprovalMinPosId,
                _appSettings.ApprovalMaxPosId);
            return (departmentHead, chiefSpecialists, groupLeaders, normContrs);
        }

        public void UpdateAll(List<EmployeeFetched> employeesFetched)
        {
            var employees = _repository.GetAll();
            // Delete should be cascade if it's necessary
            // foreach (var employee in employees)
            // {
            //     if (!employeesFetched.Select(v => v.Id).Contains(employee.Id))
            //         _repository.Delete(employee);
            // }
            foreach (var employeeFetched in employeesFetched)
            {
                var foundEmployee = employees.SingleOrDefault(v => v.Id == employeeFetched.Id);
                if (foundEmployee == null)
                    _repository.Add(employeeFetched.ToEmployee());
                else
                {
                    var wasChanged = false;
                    if (foundEmployee.Fullname != employeeFetched.Fullname)
                    {
                        foundEmployee.Fullname = employeeFetched.Fullname;
                        wasChanged = true;
                    }
                    if (wasChanged)
                        _repository.Update(foundEmployee);
                }
            }
        }
    }
}
