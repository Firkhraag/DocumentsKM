using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Ссылочный документ марки
    public class MarkLinkedDoc
    {
        [Key]
        public int Id { get; set; }

        // Марка
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // Ссылочный документ
        [Required]
        [ForeignKey("LinkedDocId")]
        public virtual LinkedDoc LinkedDoc { get; set; }

        // Примечание
        [MaxLength(50)]
        public string Note { get; set; }
    }
}
