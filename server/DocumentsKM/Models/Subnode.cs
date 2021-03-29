using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Подузел
    public class Subnode
    {
        [Key]
        public Int32 Id { get; set; }

        // Узел
        [Required]
        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }
        public Int32 NodeId { get; set; }

        // Код
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // Название
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
