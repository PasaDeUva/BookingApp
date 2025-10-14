using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.SendMassMessage;

public class SendMassMessageCommandHandler : IRequestHandler<SendMassMessageCommand, ResponseModel<bool>>
{
    private readonly IWhatsappBotService _botManager;
    private readonly IloginService _loginManager;

    public SendMassMessageCommandHandler(IWhatsappBotService botManager, IloginService loginManager)
    {
        _botManager = botManager;
        _loginManager = loginManager;
    }

    public async Task<ResponseModel<bool>> Handle(SendMassMessageCommand command, CancellationToken cancellationToken)
    {
        //var shopId = _loginManager.GetShopIdFromToken(HttpContext.User);
        //await _botManager.SendMassMessageAsync(shopId, command.Request.Message);
        return new ResponseModel<bool> { Success = true};
    }
}
