using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Название организации
    public class OrganizationName
    {
        [Required]
        [MaxLength(255)]
        public String Model { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string ShortName { get; set; }
    }
}
