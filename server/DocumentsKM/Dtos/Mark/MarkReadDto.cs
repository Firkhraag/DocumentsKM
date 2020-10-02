using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkReadDto
    {
        public ulong Id { get; set; }

        public Subnode Subnode { get; set; }

        public string Code { get; set; }

        public string AdditionalCode { get; set; }

        public string Name { get; set; }

        public Department Department { get; set; }

        public Employee ChiefSpecialist { get; set; }

        public Employee GroupLeader { get; set; }

        public Employee MainBulder { get; set; }

        public Employee ApprovalSpecialist1 { get; set; }

        public Employee ApprovalSpecialist2 { get; set; }

        public Employee ApprovalSpecialist3 { get; set; }

        public Employee ApprovalSpecialist4 { get; set; }

        public Employee ApprovalSpecialist5 { get; set; }

        public Employee ApprovalSpecialist6 { get; set; }

        public Employee ApprovalSpecialist7 { get; set; }
    }
}
