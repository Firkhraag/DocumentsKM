using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class ConstructionSubtype
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TypeId")]
        public virtual ConstructionType Type { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Valuation { get; set; }
    }
}
