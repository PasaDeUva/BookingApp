using BotWhatsapp.Application.Features.Login.ChangePassword;
using BotWhatsapp.Application.Features.Login.Login;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BotWhatsapp.Application.Services;
public class LoginService : IloginService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public LoginService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<ResponseModel<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new ResponseModel<LoginResponse>
            {
                Success = false,
                Data = null,
                Description = "Usuario o contraseña incorrectos"
            };
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        var res = await _userRepository.UpdateAsync(user);

        return new ResponseModel<LoginResponse>
        {
            Success = res > 0,
            Data = new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Name = user.Username
            },
            Description = res > 0 ? "Login exitoso" : "Error al actualizar el usuario"
        };

    }
    public async Task<ResponseModel<LoginResponse>> RefreshTokenAsync(string refreshToken)
    {
        // Buscar usuario por refresh token
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Token inválido o vencido");

        var newToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        var res = await _userRepository.UpdateAsync(user);

        return new ResponseModel<LoginResponse>
        {
            Success = res > 0,
            Data = new LoginResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Name = user.Username
            },
            Description = res > 0 ? "Token actualizado exitosamente" : "Error al actualizar el usuario"
        };
    }

    public async Task<ResponseModel<bool>> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Description = "Usuario no encontrado" };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "Contraseña actual incorrecta"
                };
            }

            user.PasswordHash = GeneratePasswordHash(request.NewPassword);
            var res = await _userRepository.UpdateAsync(user);

            return new ResponseModel<bool>
            {
                Success = res > 0,
                Data = res > 0,
                Description = res > 0 ? "Contraseña actualizada exitosamente" : "Error al actualizar la contraseña"
            };

        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al cambiar la contraseña"
            };

        }
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(14),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private static string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }
}