using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;
using Serilog;

namespace DocumentsKM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepo _repository;

        public EmployeeService(IEmployeeRepo EmployeeRepo)
        {
            _repository = EmployeeRepo;
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
            // У каждого отдела должен быть один руководитель
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();
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
    }
}
