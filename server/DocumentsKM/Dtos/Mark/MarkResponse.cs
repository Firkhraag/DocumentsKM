namespace DocumentsKM.Dtos
{
    public class MarkResponse : MarkBaseResponse
    {
        public string Name { get; set; }
        public SubnodeResponse Subnode { get; set; }
    }
}
