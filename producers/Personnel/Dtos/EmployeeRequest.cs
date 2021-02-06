using System.ComponentModel.DataAnnotations;

namespace Personnel.Dtos
{
    public class EmployeeRequest
    {
        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
    }
}
