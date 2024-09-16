namespace MyApp.DTOs
{
    public class LoginResponseDto
    {
        public bool IsLoggedIn { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
