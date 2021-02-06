using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class IdRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
