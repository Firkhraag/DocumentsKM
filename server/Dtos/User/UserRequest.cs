using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class UserRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
