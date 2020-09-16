using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Model
{
    public class Position
    {
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
