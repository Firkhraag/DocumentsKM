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

        public IEnumerable<Employee> GetMarkApprovalEmployeesByDepartmentNumber(int departmentNumber)
        {
            int minPosCode = 1170;
            int maxPosCode = 1251;
            var employees = _repository.GetAllByDepartmentNumberAndPositionRange(
                departmentNumber,
                minPosCode,
                maxPosCode);
            return employees;
        }

        public (IEnumerable<Employee>,IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentNumber(
            int departmentNumber)
        {
            var chiefSpecialistPosCode = 1100;
            var groupLeaderPosCode = 1185;
            var mainBuilderPosCode = 1285;

            var chiefSpecialists = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                chiefSpecialistPosCode);
            var groupLeaders = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                groupLeaderPosCode);
            var mainBuilders = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                mainBuilderPosCode);

            return (chiefSpecialists, groupLeaders, mainBuilders);
        }
    }
}
