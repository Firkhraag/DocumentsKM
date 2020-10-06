using System.Collections.Generic;

namespace DocumentsKM.Dtos
{
    public class MarkMainEmployeesResponse
    {
        public IEnumerable<EmployeeBaseResponse> ChiefSpecialists { get; set; }
        public IEnumerable<EmployeeBaseResponse> GroupLeaders { get; set; }
        public IEnumerable<EmployeeBaseResponse> MainBuilders { get; set; }

        public MarkMainEmployeesResponse(
            IEnumerable<EmployeeBaseResponse> chiefSpecialists,
            IEnumerable<EmployeeBaseResponse> groupLeaders,
            IEnumerable<EmployeeBaseResponse> mainBuilders)
        {
            ChiefSpecialists = chiefSpecialists;
            GroupLeaders = groupLeaders;
            MainBuilders = mainBuilders;
        }
    }
}
