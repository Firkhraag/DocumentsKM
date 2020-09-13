using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ProjectKM
{
    public class AttachedDocuments
    {
        // For now we will consider NOT NULL constraint for every field

        // Поз_прил
        [Key]
        public ulong Position { get; set; }

        // Id_марки
        // FOREIGN KEY т. Марки
        [Required]
        public ulong MarkId { get; set; }

        // обозн_прил
        [Required]
        [MaxLength(100)]
        public string Notation { get; set; }

        // CREATE UNIQUE INDEX <name> ON (Id_марки, обозн_прил)

        // наим_прил
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        // прим
        [Required]
        [MaxLength(50)]
        public string Note { get; set; }
    }
}