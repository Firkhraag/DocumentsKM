using System.Collections.Generic;

namespace DocumentsKM.Dtos
{
    public class MarkMainEmployeesResponse
    {
        public EmployeeBaseResponse DepartmentHead { get; set; }
        public IEnumerable<EmployeeBaseResponse> ChiefSpecialists { get; set; }
        public IEnumerable<EmployeeBaseResponse> GroupLeaders { get; set; }
        public IEnumerable<EmployeeBaseResponse> MainBuilders { get; set; }

        public MarkMainEmployeesResponse(
            EmployeeBaseResponse departmentHead,
            IEnumerable<EmployeeBaseResponse> chiefSpecialists,
            IEnumerable<EmployeeBaseResponse> groupLeaders,
            IEnumerable<EmployeeBaseResponse> mainBuilders)
        {
            DepartmentHead = departmentHead;
            ChiefSpecialists = chiefSpecialists;
            GroupLeaders = groupLeaders;
            MainBuilders = mainBuilders;
        }
    }
}
