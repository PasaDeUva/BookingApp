using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Login.RefreshToken;

public class RefreshTokenRequest : IRequest<ResponseModel<LoginResponse>>
{
    public string Token { get; set; }
}
