using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Models
{
    // Пользователь
    public class User
    {
        [Key]
        public int Id { get; set; }

        // Логин
        [Required]
        [MaxLength(255)]
        public string Login { get; set; }

        // Пароль
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        
        // Сотрудник
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
