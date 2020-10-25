using System.Collections.Generic;
using DocumentsKM.Models;
using DocumentsKM.Data;
using System.Linq;

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

        public (Employee, IEnumerable<Employee>,IEnumerable<Employee>, IEnumerable<Employee>) GetMarkMainEmployeesByDepartmentNumber(
            int departmentNumber)
        {
            var departmentHeadPosCode = 1290;
            var chiefSpecialistPosCode = 1100;
            var groupLeaderPosCode = 1185;
            var mainBuilderPosCode = 1285;

            var departmentHeadArr = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                departmentHeadPosCode);
            if (departmentHeadArr.Count() != 1)
                throw new ConflictException();
            var departmentHead = departmentHeadArr.ToList()[0];
            var chiefSpecialists = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                chiefSpecialistPosCode);
            var groupLeaders = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                groupLeaderPosCode);
            var mainBuilders = _repository.GetAllByDepartmentNumberAndPosition(
                departmentNumber,
                mainBuilderPosCode);
            return (departmentHead, chiefSpecialists, groupLeaders, mainBuilders);
        }
    }
}
