using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkParentResponse : MarkBaseResponse
    {
        public Subnode Subnode { get; set; }
    }
}
