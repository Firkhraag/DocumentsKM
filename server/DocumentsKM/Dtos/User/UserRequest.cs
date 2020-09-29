using System.ComponentModel.DataAnnotations;

namespace DocumentsKM.Dtos
{
    public class UserRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Login { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Password { get; set; }

        public UserRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
