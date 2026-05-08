namespace DOMAIN.Entities.PLECSYS_Records
{
    public class OAuthLoginRequest
    {
        public string GrantType { get; set; } = "password";
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
