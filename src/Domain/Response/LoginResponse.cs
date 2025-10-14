using BotWhatsapp.Application.DTOs;

namespace BotWhatsapp.Domain.Response;

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Name { get; set; }
}

