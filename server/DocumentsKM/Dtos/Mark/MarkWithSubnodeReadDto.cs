namespace DocumentsKM.Dtos
{
    public class MarkWithSubnodeReadDto
    {
        public ulong Id { get; set; }

        public SubnodeWithNodeReadDto Subnode { get; set; }

        public string Code { get; set; }
    }
}
