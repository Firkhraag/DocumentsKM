using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Personnel
{
    public class Position
    {
        // For now we will consider NOT NULL constraint for every field

        // КодДолжности
        [Key]
        public uint Code { get; set; }

        // НазваниеДолжности
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // ДолжностьИмя_кр
        [Required]
        [MaxLength(12)]
        public string ShortName { get; set; }
    }
}