using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Personnel
{
    public class Depatment
    {
        // For now we will consider NOT NULL constraint for every field

        // НомОтдела
        [Key]
        public ulong Number { get; set; }

        // НазваниеОтдела
        [Required]
        [MaxLength(140)]
        public string Name { get; set; }

        // НазваниеОтделаК
        [Required]
        [MaxLength(40)]
        public string ShortName { get; set; }

        // КодОтдела
        [Required]
        [MaxLength(5)]
        public string Code { get; set; }

        // Активность
        [Required]
        public bool IsActive { get; set; }

        // Производственный
        [Required]
        public bool IsIndustrial { get; set; }
    }
}