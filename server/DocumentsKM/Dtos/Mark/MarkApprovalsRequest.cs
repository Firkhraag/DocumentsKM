namespace DocumentsKM.Dtos
{
    public class MarkApprovalsRequest
    {
        public int? ApprovalSpecialist1Id { get; set; }
        public int? ApprovalSpecialist2Id { get; set; }
        public int? ApprovalSpecialist3Id { get; set; }
        public int? ApprovalSpecialist4Id { get; set; }
        public int? ApprovalSpecialist5Id { get; set; }
        public int? ApprovalSpecialist6Id { get; set; }
        public int? ApprovalSpecialist7Id { get; set; }

         public MarkApprovalsRequest()
        {
            ApprovalSpecialist1Id = null;
            ApprovalSpecialist2Id = null;
            ApprovalSpecialist3Id = null;
            ApprovalSpecialist4Id = null;
            ApprovalSpecialist5Id = null;
            ApprovalSpecialist6Id = null;
            ApprovalSpecialist7Id = null;
        }
    }
}
