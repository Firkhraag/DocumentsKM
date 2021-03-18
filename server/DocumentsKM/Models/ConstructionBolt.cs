using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Высокопрочный болт
    public class ConstructionBolt
    {
        [Key]
        public Int16 Id { get; set; }

        // Вид конструкции
        [Required]
        [ForeignKey("ConstructionId")]
        public virtual Construction Construction { get; set; }

        // Диаметр
        [Required]
        [ForeignKey("DiameterId")]
        public virtual BoltDiameter Diameter { get; set; }

        // Толщина пакета
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Packet { get; set; }

        // Количество болтов
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 Num { get; set; }

        // Количество гаек
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 NutNum { get; set; }

        // Количество шайб
        [Required]
        [Range(0, 65535, ErrorMessage = "Value should be greater than or equal to 0")]
        public Int16 WasherNum { get; set; }
    }
}
