using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkParentResponse : MarkBaseResponse
    {
        // public SubnodeParentResponse Subnode { get; set; }
        public Subnode Subnode { get; set; }
    }
}
