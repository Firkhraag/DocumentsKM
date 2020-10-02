using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    public class Specification
    {
        // Поз_выпуска
        [Key]
        public int Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        [ForeignKey("MarkId")]
        public virtual Mark Mark { get; set; }

        // выпуск
        [Required]
        public byte ReleaseNumber { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, выпуск)

        // прим
        [MaxLength(255)]
        public string Note { get; set; }

        // дата_созд
        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
    }
}
