using System.Text.Json.Serialization;

namespace DocumentsKM.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }

        // Refresh token возвращается только в http only cookie
        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public UserResponse(int id, string fullName, string token, string refreshToken)
        {
            Id = id;
            FullName = fullName;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
