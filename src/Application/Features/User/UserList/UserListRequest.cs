using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.UserList;

public class UserListRequest : PaginationRequest, IRequest<ResponseModel<PaginationResponse<UserDto>>>
{
    public int? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}
