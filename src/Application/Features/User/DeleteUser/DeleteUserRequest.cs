using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.DeleteUser;

public class DeleteUserRequest : IRequest<ResponseModel<bool>>
{
    public int UserId { get; set; }
}
