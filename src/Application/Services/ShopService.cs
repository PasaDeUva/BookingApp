using BotWhatsapp.Application.Features.Shop.Create;
using BotWhatsapp.Application.Features.Shop.GetList;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;
using System.Security.Claims;

namespace BotWhatsapp.Application.Services;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;

    public ShopService(IShopRepository shopRepository)
    {
        _shopRepository = shopRepository;
    }
    public async Task<ResponseModel<PaginationResponse<ShopDto>>> GetAllAsync(GetShopsRequest request)
    {
        try
        {
            var shops = await _shopRepository.GetAllAsync(request.PageNumber, request.PageSize, request.Id, request.Name, request.Instance,
                                                          request.IsActive, request.AssistanceEnable);
            return new ResponseModel<PaginationResponse<ShopDto>>
            {
                Success = true,
                Data = new PaginationResponse<ShopDto>
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = shops.Item2,
                    Data = shops.Item1.Select(shop => new ShopDto
                    {
                        Id = shop.Id,
                        Instance = shop.Instance,
                        Name = shop.Name,
                        AssistanceEnable = shop.AssistanceEnable,
                        IsActive = shop.IsActive,
                    }).ToList()
                },
            };
        }
        catch (Exception)
        {
            return new ResponseModel<PaginationResponse<ShopDto>>
            {
                Success = false,
                Data = null,
                Description = "Ocurrio un error al obtener la lista de negocios"
            };
        }
    }
    public async Task<ResponseModel<bool>> CreateAsync(CreateShopRequest request)
    {
        try
        {
            var shop = new Shop
            {
                Instance = request.Instance,
                Name = request.Name,
                AssistanceEnable = request.AssistanceEnable,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var shopC = await _shopRepository.CreateAsync(shop);

            return new ResponseModel<bool>
            {
                Success = shopC > 0,
                Data = shopC > 0,
                Description = shopC > 0 ? "El negocio fue creado exitosamente" : "El negocio no fue creado"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Ocurri� un error al crear el negocio"
            };

        }
    }
    public async Task<ResponseModel<bool>> UpdateAsync(UpdateShopRequest request)
    {
        try
        {
            var shop = new Shop
            {
                Id = request.Id,
                Instance = request.Instance,
                Name = request.Name,
                AssistanceEnable = request.AssistanceEnable,
                IsActive = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            var shopU = await _shopRepository.UpdateAsync(shop);
            return new ResponseModel<bool>
            {
                Success = shopU > 0,
                Data = shopU > 0,
                Description = shopU > 0 ? "El negocio fue actualizado exitosamente" : "El negocio no fue actualizado"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Ocurri� un error al actualizar el negocio"
            };
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            var shopD = await _shopRepository.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = shopD > 0,
                Data = shopD > 0,
                Description = shopD > 0 ? "El negocio fue eliminado exitosamente" : "El negocio no fue eliminado"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Ocurri� un error al eliminar el negocio"
            };
        }
    }
    public async Task<ResponseModel<bool>> GetAssistanceStatus()
    {
        try
        {
            var status = await _shopRepository.GetAssistanceStatus();
            return new ResponseModel<bool>
            {
                Success = true,
                Data = status,
                Description = status ? "Asistencia del bot habilitada" : "Asistencia del bot deshabilitada"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Ocurri� un error al obtener el estado de asistencia del bot"
            };
        }
    }

    public async Task<ResponseModel<bool>> ToggleBotAssistanceAsync()
    {
        try
        {
            var shopU = await _shopRepository.UpdateAssistanceEnableAsync();
            return new ResponseModel<bool>
            {
                Success = shopU > 0,
                Data = shopU > 0,
                Description = shopU > 0 ? "Asistencia del bot actualizada exitosamente" : "Asistencia del bot no actualizada"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Ocurri� un error al actualizar la asistencia del bot"
            };
        }
    }

    public async Task<ResponseModel<Shop?>> GetByIdAsync(int id)
    {
        try
        {
            var shop = await _shopRepository.GetByIdAsync(id);
            return new ResponseModel<Shop?>
            {
                Success = shop != null,
                Data = shop,
                Description = shop != null ? "Negocio encontrado" : "Negocio no encontrado"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<Shop?>
            {
                Success = false,
                Data = null,
                Description = "Ocurri� un error al obtener el negocio"
            };
        }
    }

    public int GetShopIdFromToken(ClaimsPrincipal user)
    {
        var shopIdClaim = user.FindFirst("ShopId");
        if (shopIdClaim == null)
            throw new UnauthorizedAccessException("Token no contiene ShopId");

        if (!int.TryParse(shopIdClaim.Value, out int shopId))
            throw new UnauthorizedAccessException("ShopId inválido en el token");

        return shopId;
    }
}