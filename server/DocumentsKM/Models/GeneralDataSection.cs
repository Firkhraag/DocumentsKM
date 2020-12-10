using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class GeneralDataSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int OrderNum { get; set; }
    }
}