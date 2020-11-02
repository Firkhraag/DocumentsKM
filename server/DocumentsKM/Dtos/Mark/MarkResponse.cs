using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkResponse : MarkBaseResponse
    {
        public string Name { get; set; }
        public Subnode Subnode { get; set; }
        public virtual EmployeeBaseResponse ChiefSpecialist { get; set; }
        public virtual EmployeeBaseResponse GroupLeader { get; set; }
        public virtual EmployeeBaseResponse MainBuilder { get; set; }
    }
}
