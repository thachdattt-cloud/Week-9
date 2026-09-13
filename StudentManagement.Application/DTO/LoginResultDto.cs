namespace StudentManagement.Application.DTO
{
    public class LoginResultDto
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}