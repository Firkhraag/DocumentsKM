using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Вид ссылочного документа
    public class LinkedDocType
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
