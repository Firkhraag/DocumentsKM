using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class MarkGeneralDataPoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        [Required]
        [ForeignKey("SectionId")]
        public virtual GeneralDataSection Section { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int OrderNum { get; set; }
    }
}