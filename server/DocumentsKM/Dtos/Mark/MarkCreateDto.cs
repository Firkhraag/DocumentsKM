using System;
using System.ComponentModel.DataAnnotations;
using DocumentsKM.Model;

namespace DocumentsKM.Dtos
{
    public class MarkCreateDto
    {
        [Required]
        public Subnode Subnode { get; set; }

        [Required]
        [MaxLength(40)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string AdditionalCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }

        public Employee ChiefSpecialist { get; set; }

        public Employee GroupLeader { get; set; }

        [Required]
        public Employee MainBulder { get; set; }

        public Employee AgreedWorker1 { get; set; }

        public Employee AgreedWorker2 { get; set; }

        public Employee AgreedWorker3 { get; set; }

        public Employee AgreedWorker4 { get; set; }

        public Employee AgreedWorker5 { get; set; }

        public Employee AgreedWorker6 { get; set; }

        public Employee AgreedWorker7 { get; set; }
    }
}
