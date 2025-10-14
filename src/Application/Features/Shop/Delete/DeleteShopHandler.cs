using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.Delete;

public class DeleteShopHandler : IRequestHandler<DeleteShopRequest, ResponseModel<bool>>
{
    private readonly IShopService _shopManager;
    public DeleteShopHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }
    public async Task<ResponseModel<bool>> Handle(DeleteShopRequest request, CancellationToken cancellationToken)
    {
        return await _shopManager.DeleteAsync(request.Id);
    }
}
