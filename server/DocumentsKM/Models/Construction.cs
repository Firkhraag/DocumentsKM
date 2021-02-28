using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsKM.Models
{
    // Вид конструкции
    public class Construction
    {
        [Key]
        public int Id { get; set; }

        // Выпуск спецификации
        [Required]
        [ForeignKey("SpecificationId")]
        public virtual Specification Specification { get; set; }

        // Наименование
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Вид
        [Required]
        [ForeignKey("TypeId")]
        public virtual ConstructionType Type { get; set; }

        // Подвид
        [ForeignKey("SubtypeId")]
        public virtual ConstructionSubtype Subtype { get; set; }
        public int? SubtypeId { get; set; }

        // Расценка
        [MaxLength(10)]
        public string Valuation { get; set; }

        // Шифр типового альбома
        [MaxLength(20)]
        public string StandardAlbumCode { get; set; }

        // Количество типовых конструкций
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public int NumOfStandardConstructions { get; set; }

        // Притупление кромок
        [Required]
        public bool HasEdgeBlunting { get; set; }

        // Динамическая нагрузка
        [Required]
        public bool HasDynamicLoad { get; set; }

        // Фланцевые соединения
        [Required]
        public bool HasFlangedConnections { get; set; }

        // Контроль плотности сварных швов
        [Required]
        [ForeignKey("WeldingControlId")]
        public virtual WeldingControl WeldingControl { get; set; }

        // Коэффициент окрашивания
        [Required]
        [Range(0, 1000000, ErrorMessage = "Value should be greater than or equal to 0")]
        public float PaintworkCoeff { get; set; }

        // Элементы вида конструкции
        public virtual IList<ConstructionElement> ConstructionElements { get; set; } = new List<ConstructionElement>();

        // Высокопрочные болты
        public virtual IList<ConstructionBolt> ConstructionBolts { get; set; } = new List<ConstructionBolt>();
    }
}
