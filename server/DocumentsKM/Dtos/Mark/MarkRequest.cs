using System.ComponentModel.DataAnnotations;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkRequest
    {
        [Required]
        public Subnode Subnode { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }

        public Employee ChiefSpecialist { get; set; }

        public Employee GroupLeader { get; set; }

        [Required]
        public Employee MainBulder { get; set; }
    }
}
