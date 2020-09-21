using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Model
{
    public class Subnode
    {
        // Подузел
        [Key]
        public ulong Id { get; set; }

        // Узел
        [Required]
        [ForeignKey("NodeId")]
        public Node Node { get; set; }

        // КодПодуз
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        // НазвПодузла
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // НазвПодузлаДоп
        [Required]
        [MaxLength(255)]
        public string AdditionalName { get; set; }

        // ДатаПодуз
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // public List<Mark> Marks { get; set; }
    }
}
