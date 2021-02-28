using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Марка стали
    public class Steel
    {
        [Key]
        public int Id { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // ГОСТ
        [Required]
        [MaxLength(50)]
        public string Standard { get; set; }

        // Прочность
        public int? Strength { get; set; }
    }
}
