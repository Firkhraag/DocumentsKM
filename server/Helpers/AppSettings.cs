namespace DocumentsKM.Helpers
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public int AccessTokenExpireTimeInMinutes { get; set; }
        public int RefreshTokenExpireTimeInDays { get; set; }
        public byte TokensRedisDbNumber { get; set; }
    }
}
