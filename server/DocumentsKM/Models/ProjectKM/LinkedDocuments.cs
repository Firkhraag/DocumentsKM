using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ProjectKM
{
    public class LinkedDocuments
    {
        // For now we will consider NOT NULL constraint for every field

        // Поз_сд
        [Key]
        public ulong Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // шифр_сд
        // FOREIGN KEY т. Спр_ссыл
        [Required]
        [MaxLength(4)]
        public string Cipher { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, шифр_сд)

        // прим
        [Required]
        [MaxLength(50)]
        public string Note { get; set; }
    }
}