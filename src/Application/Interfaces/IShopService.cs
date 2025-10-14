using BotWhatsapp.Application.Features.Shop.Create;
using BotWhatsapp.Application.Features.Shop.GetList;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using System.Security.Claims;

namespace BotWhatsapp.Application.Interfaces;

public interface IShopService
{
    Task<ResponseModel<PaginationResponse<ShopDto>>> GetAllAsync(GetShopsRequest request);
    Task<ResponseModel<bool>> CreateAsync(CreateShopRequest request);
    Task<ResponseModel<bool>> UpdateAsync(UpdateShopRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<Shop?>> GetByIdAsync(int id);
    Task<ResponseModel<bool>> ToggleBotAssistanceAsync();
    Task<ResponseModel<bool>> GetAssistanceStatus();
    int GetShopIdFromToken(ClaimsPrincipal user);

}