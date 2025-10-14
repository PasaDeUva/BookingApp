using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.GetById;

public class GetByIdHandler : IRequestHandler<GetByIdRequest, ResponseModel<ShopDto>>
{
    private readonly IShopService _shopManager;
    public GetByIdHandler(IShopService shopManager)
    {
        _shopManager = shopManager;
    }

    public async Task<ResponseModel<ShopDto>> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _shopManager.GetByIdAsync(request.Id);

            var dto = new ShopDto
            {
                Id = response.Data.Id,
                Instance = response.Data.Instance,
                Name = response.Data.Name,
                AssistanceEnable = response.Data.AssistanceEnable,
                IsActive = response.Data.IsActive
            };

            return new ResponseModel<ShopDto>
            {
                Success = response.Success,
                Data = dto,
                Description = response.Description
            };
        }
        catch (Exception)
        {
            return new ResponseModel<ShopDto>
            {
                Success = false,
                Data = null,
                Description = "An error occurred while retrieving the shop"
            };
        }
    }
}
