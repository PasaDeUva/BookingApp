using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.AssistanceStatus;

public class AssistanceStatusQueryHandler : IRequestHandler<AssistanceStatusQuery, ResponseModel<bool>>
{
    private readonly IShopService _shopManager;

    public AssistanceStatusQueryHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }

    public async Task<ResponseModel<bool>> Handle(AssistanceStatusQuery query, CancellationToken cancellationToken)
    {
        var status = await _shopManager.GetAssistanceStatus();
        return status;
    }
}
