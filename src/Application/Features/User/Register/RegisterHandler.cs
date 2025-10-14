using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.User.Register;

public class RegisterHandler : IRequestHandler<RegisterRequest, ResponseModel<bool>>
{
    private readonly IUserService _userManager;

    public RegisterHandler(IUserService userManager)
    {
        _userManager = userManager;
    }
    public async Task<ResponseModel<bool>> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _userManager.CreateAsync(request);
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "An error occurred while processing your request. " + ex.Message
            };
        }
    }
}
