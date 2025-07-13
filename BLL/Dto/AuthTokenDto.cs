namespace BLL.Dto
{
    public class AuthTokenDto
    {
        public required string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
