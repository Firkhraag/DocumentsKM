using System.Text.Json.Serialization;

namespace DocumentsKM.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }

        // Refresh token возвращается только в cookie
        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public UserResponse(int id, string name, string acessToken, string refreshToken)
        {
            Id = id;
            Name = name;
            AccessToken = acessToken;
            RefreshToken = refreshToken;
        }

        public UserResponse() {}
    }
}
