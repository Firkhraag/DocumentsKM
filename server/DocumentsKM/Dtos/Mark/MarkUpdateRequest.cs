namespace DocumentsKM.Dtos
{
    public class MarkUpdateRequest
    {
        public int? SubnodeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? ChiefSpecialistId { get; set; }
        public int? GroupLeaderId { get; set; }
        public int? MainBuilderId { get; set; }

         public MarkUpdateRequest()
        {
            SubnodeId = null;
            Code = null;
            Name = null;
            DepartmentId = null;
            ChiefSpecialistId = null;
            GroupLeaderId = null;
            MainBuilderId = null;
        }
    }
}
