using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class CorrProtCleaningDegree
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
