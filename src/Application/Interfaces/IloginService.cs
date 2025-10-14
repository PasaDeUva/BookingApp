using BotWhatsapp.Application.Features.Login.ChangePassword;
using BotWhatsapp.Application.Features.Login.Login;
using BotWhatsapp.Domain.Response;
using System.Security.Claims;

namespace BotWhatsapp.Application.Interfaces;

public interface IloginService
{
    Task<ResponseModel<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ResponseModel<LoginResponse>> RefreshTokenAsync(string token);
    Task<ResponseModel<bool>> ChangePasswordAsync(ChangePasswordRequest request);
}
