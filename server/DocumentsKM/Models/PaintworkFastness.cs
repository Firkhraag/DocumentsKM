using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Стойкость лакокрасочных покрытий
    public class PaintworkFastness
    {
        [Key]
        public int Id { get; set; }

        // Название
        [Required]
        [MaxLength(2)]
        public string Name { get; set; }
    }
}
