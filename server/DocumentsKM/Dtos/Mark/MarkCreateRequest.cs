using System;
using System.ComponentModel.DataAnnotations;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class MarkCreateRequest
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

        public Employee ApprovalSpecialist1 { get; set; }

        public Employee ApprovalSpecialist2 { get; set; }

        public Employee ApprovalSpecialist3 { get; set; }

        public Employee ApprovalSpecialist4 { get; set; }

        public Employee ApprovalSpecialist5 { get; set; }

        public Employee ApprovalSpecialist6 { get; set; }

        public Employee ApprovalSpecialist7 { get; set; }
    }
}
