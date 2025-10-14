using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.UserList;

public class UserListHandler : IRequestHandler<UserListRequest, ResponseModel<PaginationResponse<UserDto>>>
{        
    private readonly IUserService _userManager;

    public UserListHandler(IUserService userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseModel<PaginationResponse<UserDto>>> Handle(UserListRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _userManager.GetAllAsync(request);
        }
        catch (Exception ex)
        {
            return new ResponseModel<PaginationResponse<UserDto>>
            {
                Success = false,
                Data = null,
                Description = "An error occurred while processing your request. " + ex.Message
            };
        }
    }
}
