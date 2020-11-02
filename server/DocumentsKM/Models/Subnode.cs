using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Subnode
    {
        // Подузел
        [Key]
        public int Id { get; set; }

        // Узел
        [Required]
        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }

        // КодПодуз
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // НазвПодузла
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
