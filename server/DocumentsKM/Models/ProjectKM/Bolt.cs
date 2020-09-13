using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.ProjectKM
{
    public class Bolt
    {
        // For now we will consider NOT NULL constraint for every field

        // Код_д
        [Key]
        public ulong Code { get; set; }

        // диаметр
        [Required]
        public byte Diameter { get; set; }

        // ТУ_б
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        // прим
        [Required]
        [MaxLength(50)]
        public string Note { get; set; }
    }
}