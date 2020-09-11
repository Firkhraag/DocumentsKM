using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class AdditionalWork
    {
        // For now we will consider NOT NULL constraint for every field

        // Поз_вып_доп
        [Key]
        public ulong Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // Исп
        // FOREIGN_KEY из таблицы СписокРаботников
        [Required]
        public ulong Worker { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, Исп)

        // Расчет
        [Required]
        public uint Estimation { get; set; }

        // Расчет
        [Required]
        public uint Order { get; set; }
    }
}