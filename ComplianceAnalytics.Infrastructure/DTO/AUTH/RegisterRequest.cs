namespace ComplianceAnalytics.Infrastructure.DTO.Auth
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? Region { get; set; }
    }
}
