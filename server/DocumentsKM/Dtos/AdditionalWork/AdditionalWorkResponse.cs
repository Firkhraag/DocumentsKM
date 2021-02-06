namespace DocumentsKM.Dtos
{
    public class AdditionalWorkResponse
    {
        public int Id { get; set; }
        public EmployeeBaseResponse Employee { get; set; }
        public int Valuation { get; set; }
        public int MetalOrder { get; set; }
        public float DrawingsCompleted { get; set; }
        public float DrawingsCheck { get; set; }
    }
}
