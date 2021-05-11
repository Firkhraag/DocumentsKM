namespace DocumentsKM.Dtos
{
    public class MarkParentResponse
    {
        public MarkResponse Mark { get; set; }
        public int SubnodeId { get; set; }
        public int NodeId { get; set; }
        public int ProjectId { get; set; }
    }
}
