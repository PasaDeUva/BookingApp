using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetAll;

public class GetAllClientRequest : PaginationRequest, IRequest<ResponseModel<PaginationResponse<ClientDto>>>
{
    public int? Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public bool? IsActive { get; set; } 
    public bool? IsBloqued { get; set; }   
}