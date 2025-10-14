namespace BotWhatsapp.Domain.Dtos;

public class LoginDto : UserDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
