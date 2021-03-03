using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Подузел
    public class Subnode
    {
        [Key]
        public int Id { get; set; }

        // Узел
        [Required]
        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }

        // Код
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // Название
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
