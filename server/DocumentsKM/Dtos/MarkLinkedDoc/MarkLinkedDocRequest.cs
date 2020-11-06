using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkLinkedDocRequest
    {
        [Required]
        public int LinkedDocId { get; set; }
    }
}
