using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Login.Login;

public class LoginHandler : IRequestHandler<LoginRequest, ResponseModel<LoginResponse>>
{
    private readonly IloginService _loginManager;

    public LoginHandler(IloginService loginManager)
    {
        _loginManager = loginManager;
    }

    public async Task<ResponseModel<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _loginManager.LoginAsync(request);
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