using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class GeneralDataPoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("SectionId")]
        public virtual GeneralDataSection Section { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int OrderNum { get; set; }
    }
}
