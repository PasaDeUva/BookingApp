using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.GetList;

public class GetShopsRequest : PaginationRequest, IRequest<ResponseModel<PaginationResponse<ShopDto>>>
{
    public int? Id { get; set; }
    public string? Instance { get; set; }
    public string? Name { get; set; }
    public bool? AssistanceEnable { get; set; }
    public bool? IsActive { get; set; }
}
