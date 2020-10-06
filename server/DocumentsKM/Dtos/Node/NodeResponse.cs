namespace DocumentsKM.Dtos
{
    public class NodeResponse : NodeBaseResponse
    {
        public string Name { get; set; }
        public EmployeeBaseResponse ChiefEngineer { get; set; }
        public ProjectResponse Project { get; set; }
    }
}
