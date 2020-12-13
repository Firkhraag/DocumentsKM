using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class WeldingControl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
