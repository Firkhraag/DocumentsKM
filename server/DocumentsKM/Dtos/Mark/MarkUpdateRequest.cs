namespace DocumentsKM.Dtos
{
    public class MarkUpdateRequest
    {
        public int? SubnodeId { get; set; }
        public string Code { get; set; }
        public string AdditionalCode { get; set; }
        public string Name { get; set; }
        public int? DepartmentNumber { get; set; }
        public int? ChiefSpecialistId { get; set; }
        public int? GroupLeaderId { get; set; }
        public int? MainBuilderId { get; set; }
        public int? CurrentSpecificationId { get; set; }

         public MarkUpdateRequest()
        {
            SubnodeId = null;
            Code = null;
            AdditionalCode = null;
            Name = null;
            DepartmentNumber = null;
            ChiefSpecialistId = null;
            GroupLeaderId = null;
            MainBuilderId = null;
            CurrentSpecificationId = null;
        }
    }
}
