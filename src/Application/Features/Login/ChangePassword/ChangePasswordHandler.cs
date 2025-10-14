using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Login.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, ResponseModel<bool>>
{
    private readonly IloginService _loginManager;

    public ChangePasswordHandler(IloginService loginManager)
    {
        _loginManager = loginManager;
    }

    public async Task<ResponseModel<bool>> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _loginManager.ChangePasswordAsync(request);
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al cambiar la contraseña: " + ex.Message
            };
        }
    }
}
