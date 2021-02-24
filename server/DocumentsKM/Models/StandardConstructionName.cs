using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class StandardConstructionName
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
