using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class MarkLinkedDocCreateRequest
    {
        [Required]
        public int LinkedDocId { get; set; }

        [MaxLength(50)]
        public string Note { get; set; }
    }
}
