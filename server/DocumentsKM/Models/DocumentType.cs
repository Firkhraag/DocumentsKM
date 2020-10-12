using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class DocumentType
    {
        // Тип_док
        [Key]
        public byte Type { get; set; }

        // шифр_док
        [Required]
        [MaxLength(4)]
        public string Code { get; set; }

        // название_док
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}