using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.GetList;

public class GetShopsHandler : IRequestHandler<GetShopsRequest, ResponseModel<PaginationResponse<ShopDto>>>
{
    private readonly IShopService _shopManager;
    public GetShopsHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }
    public async Task<ResponseModel<PaginationResponse<ShopDto>>> Handle(GetShopsRequest request, CancellationToken cancellationToken)
    {
        return await _shopManager.GetAllAsync(request);
    }
}
