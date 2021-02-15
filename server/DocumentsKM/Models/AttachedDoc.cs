using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Прочие прилагаемые документы
    public class AttachedDoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Mark Mark { get; set; }

        [Required]
        [MaxLength(40)]
        public string Designation { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }
    }
}
