using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Login.Login;

public class LoginRequest : IRequest<ResponseModel<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
