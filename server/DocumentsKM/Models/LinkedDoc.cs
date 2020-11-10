using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class LinkedDoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(4)]
        public string Code { get; set; }

        [Required]
        [ForeignKey("TypeId")]
        public virtual LinkedDocType Type { get; set; }

        [Required]
        [MaxLength(40)]
        public string Designation { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
