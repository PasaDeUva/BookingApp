using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, ResponseModel<bool>>
{        
    private readonly IUserService _userManager;

    public DeleteUserHandler(IUserService userManager)
    {
        _userManager = userManager;
    }


    public async Task<ResponseModel<bool>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _userManager.DeleteAsync(request.UserId);
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "An error occurred while deleting the user. " + ex.Message
            };
        }

    }
}
