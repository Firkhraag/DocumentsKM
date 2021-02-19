namespace DocumentsKM.Dtos
{
    public class EstimateTaskResponse
    {
        public string TaskText { get; set; }
        public string AdditionalText { get; set; }
        public EmployeeDepartmentResponse ApprovalEmployee { get; set; }
    }
}
