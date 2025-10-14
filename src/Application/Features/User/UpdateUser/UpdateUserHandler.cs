using System.Net.Http;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, ResponseModel<bool>>
{        
    private readonly IUserService _userManager;

    public UpdateUserHandler(IUserService userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseModel<bool>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _userManager.UpdateAsync(request);
        }
        catch (Exception ex)
        {

            return new ResponseModel<bool>
            {
                Success = false,
                Description = "An error occurred while updating the user. " + ex.Message,
                Data = false
            };
        }
    }
}
