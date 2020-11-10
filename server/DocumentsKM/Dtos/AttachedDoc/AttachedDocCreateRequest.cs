using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class AttachedDocCreateRequest
    {
        [Required]
        [MaxLength(40)]
        public string Designation { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }
    }
}
