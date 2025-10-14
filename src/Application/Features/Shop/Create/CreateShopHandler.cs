using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.Create;

public class CreateShopHandler : IRequestHandler<CreateShopRequest, ResponseModel<bool>>
{
    private readonly IShopService _shopManager;
    public CreateShopHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }
    public async Task<ResponseModel<bool>> Handle(CreateShopRequest request, CancellationToken cancellationToken)
    {
        
        return await _shopManager.CreateAsync(request);
    }
}
