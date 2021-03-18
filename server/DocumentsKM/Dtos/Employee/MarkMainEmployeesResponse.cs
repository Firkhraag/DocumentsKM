using System.Collections.Generic;

namespace DocumentsKM.Dtos
{
    public class MarkMainEmployeesResponse
    {
        public EmployeeBaseResponse DepartmentHead { get; set; }
        public IEnumerable<EmployeeBaseResponse> ChiefSpecialists { get; set; }
        public IEnumerable<EmployeeBaseResponse> GroupLeaders { get; set; }
        public IEnumerable<EmployeeBaseResponse> NormContrs { get; set; }

        public MarkMainEmployeesResponse(
            EmployeeBaseResponse departmentHead,
            IEnumerable<EmployeeBaseResponse> chiefSpecialists,
            IEnumerable<EmployeeBaseResponse> groupLeaders,
            IEnumerable<EmployeeBaseResponse> normContrs)
        {
            DepartmentHead = departmentHead;
            ChiefSpecialists = chiefSpecialists;
            GroupLeaders = groupLeaders;
            NormContrs = normContrs;
        }
    }
}
