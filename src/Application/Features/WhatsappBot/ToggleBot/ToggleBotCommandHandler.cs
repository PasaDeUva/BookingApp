using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.ToggleBot;

public class ToggleBotCommandHandler : IRequestHandler<ToggleBotCommand, ResponseModel<bool>>
{
    private readonly IShopService _shopManager;

    public ToggleBotCommandHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }

    public async Task<ResponseModel<bool>> Handle(ToggleBotCommand command, CancellationToken cancellationToken)
    {
        var result = await _shopManager.ToggleBotAssistanceAsync();
        return new ResponseModel<bool> { Data = result.Data };
    }
}
