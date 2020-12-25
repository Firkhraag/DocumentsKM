namespace DocumentsKM.Dtos
{
    public class AdditionalWorkUpdateRequest
    {
        public int? EmployeeId { get; set; }
        public int? Valuation { get; set; }
        public int? MetalOrder { get; set; }

        public AdditionalWorkUpdateRequest()
        {
            EmployeeId = null;
            Valuation = null;
            MetalOrder = null;
        }
    }
}
