using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace DocumentsKM.Models
{
    // Пользователь
    public class User
    {
        [Key]
        public Int16 Id { get; set; }

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
        public Int32 EmployeeId { get; set; }
    }
}
