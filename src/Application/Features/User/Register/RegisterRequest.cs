using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.Register;

public class RegisterRequest : IRequest<ResponseModel<bool>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}
