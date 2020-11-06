using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarkLinkedDoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        [Required]
        [ForeignKey("LinkedDocId")]
        public virtual LinkedDoc LinkedDoc { get; set; }

        // // прим
        // [MaxLength(50)]
        // public string Note { get; set; }
    }
}
