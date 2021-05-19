using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Марка
    public class ArchiveDepartment
    {
        // Код
        [Required]
        [MaxLength(8)]
        public string Code { get; set; }
    }
}
