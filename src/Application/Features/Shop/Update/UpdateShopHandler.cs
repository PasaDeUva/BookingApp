using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.Create;

public class UpdateShopHandler : IRequestHandler<UpdateShopRequest, ResponseModel<bool>>
{
    private readonly IShopService _shopManager;
    public UpdateShopHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }
    public async Task<ResponseModel<bool>> Handle(UpdateShopRequest request, CancellationToken cancellationToken)
    {
        return await _shopManager.UpdateAsync(request);
    }
}
