namespace DocumentsKM.Dtos
{
    public class SubnodeWithNodeReadDto
    {
        public ulong Id { get; set; }

        public NodeWithProjectReadDto Node { get; set; }

        public string Code { get; set; }
    }
}
