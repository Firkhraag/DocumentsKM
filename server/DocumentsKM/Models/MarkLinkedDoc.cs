using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarkLinkedDoc
    {
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }
        public int MarkId { get; set; }

        [Required]
        [ForeignKey("LinkedDocId")]
        public virtual LinkedDoc LinkedDoc { get; set; }
        public int LinkedDocId { get; set; }

        // // прим
        // [MaxLength(50)]
        // public string Note { get; set; }
    }
}
