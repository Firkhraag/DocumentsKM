using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    public class Position
    {
        // КодДолжности
        [Key]
        public int Code { get; set; }

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
