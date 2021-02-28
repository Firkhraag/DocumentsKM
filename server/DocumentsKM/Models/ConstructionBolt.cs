using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Высокопрочный болт
    public class ConstructionBolt
    {
        [Key]
        public int Id { get; set; }

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
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Packet { get; set; }

        // Количество болтов
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int Num { get; set; }

        // Количество гаек
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NutNum { get; set; }

        // Количество шайб
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int WasherNum { get; set; }
    }
}
