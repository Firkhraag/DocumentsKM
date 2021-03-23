using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Значения по умолчанию
    public class DefaultValues
    {
        // Пользователь
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Key]
        public Int16 UserId { get; set; }

        // Отдел
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public Int16? DepartmentId { get; set; }

        // Разработчик
        [ForeignKey("CreatorId")]
        public virtual Employee Creator { get; set; }
        public Int32? CreatorId { get; set; }

        // Проверщик
        [ForeignKey("InspectorId")]
        public virtual Employee Inspector { get; set; }
        public Int32? InspectorId { get; set; }

        // Нормоконтролер
        [ForeignKey("NormContrId")]
        public virtual Employee NormContr { get; set; }
        public Int32? NormContrId { get; set; }
    }
}
