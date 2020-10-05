namespace DocumentsKM.Dtos
{
    public class MarkResponse
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public SubnodeResponse Subnode { get; set; }
    }
}
