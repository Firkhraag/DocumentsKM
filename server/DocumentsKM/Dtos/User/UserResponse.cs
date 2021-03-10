using System.Text.Json.Serialization;
using DocumentsKM.Models;

namespace DocumentsKM.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        // public string Name { get; set; }
        public EmployeeDepartmentResponse Employee { get; set; }
        public string AccessToken { get; set; }

        // Refresh token возвращается только в cookie
        [JsonIgnore] 
        public string RefreshToken { get; set; }

        // public UserResponse(int id, string name, string acessToken, string refreshToken)
        public UserResponse(int id, Employee employee, string acessToken, string refreshToken)
        {
            Id = id;
            Employee = new EmployeeDepartmentResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
            };
            // Name = name;
            AccessToken = acessToken;
            RefreshToken = refreshToken;
        }

        public UserResponse() {}
    }
}
