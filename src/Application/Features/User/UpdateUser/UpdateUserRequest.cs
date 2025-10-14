using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.UpdateUser;

public class UpdateUserRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
