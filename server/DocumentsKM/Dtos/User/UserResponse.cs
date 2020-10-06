using System.Text.Json.Serialization;

namespace DocumentsKM.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }

        // Refresh token возвращается только в cookie
        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public UserResponse(int id, string fullName, string acessToken, string refreshToken)
        {
            Id = id;
            FullName = fullName;
            AccessToken = acessToken;
            RefreshToken = refreshToken;
        }

        public UserResponse() {}
    }
}
