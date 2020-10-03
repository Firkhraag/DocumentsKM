using System.Collections.Generic;

namespace DocumentsKM.Dtos
{
    public class MarkMainEmployeesResponse
    {
        public IEnumerable<EmployeeNameResponse> ChiefSpecialists { get; set; }

        public IEnumerable<EmployeeNameResponse> GroupLeaders { get; set; }

        public IEnumerable<EmployeeNameResponse> MainBuilders { get; set; }

        public MarkMainEmployeesResponse(
            IEnumerable<EmployeeNameResponse> chiefSpecialists,
            IEnumerable<EmployeeNameResponse> groupLeaders,
            IEnumerable<EmployeeNameResponse> mainBuilders)
        {
            ChiefSpecialists = chiefSpecialists;
            GroupLeaders = groupLeaders;
            MainBuilders = mainBuilders;
        }
    }
}