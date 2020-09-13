using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ProjectKM
{
    public class SpecificationRelease
    {
        // For now we will consider NOT NULL constraint for every field

        // Поз_выпуска
        [Key]
        public ulong Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // выпуск
        [Required]
        public byte ReleaseNumber { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, выпуск)

        // дата_созд
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        // прим
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }
    }
}