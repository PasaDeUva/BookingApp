using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Login.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, ResponseModel<LoginResponse>>
{
    private readonly IloginService _loginManager;

    public RefreshTokenHandler(IloginService loginManager)
    {
        _loginManager = loginManager;
    }

    public async Task<ResponseModel<LoginResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _loginManager.RefreshTokenAsync(request.Token);
        }
        catch (Exception ex)
        {
            return new ResponseModel<LoginResponse>
            {
                Success = false,
                Data = null,
                Description = "Error al iniciar sesión: " + ex.Message
            };
        }
    }
}
