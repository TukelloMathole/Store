namespace StoreApp.DTOs
{
    public class LoginResponseDto
    {
        public bool IsLoggedIn { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
