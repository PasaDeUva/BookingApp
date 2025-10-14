
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.GetByResource;

public class GetServiceByResourceRequest : IRequest<ResponseModel<List<ServiceDto>>>
{
    public int ResourceId { get; set; }
    public GetServiceByResourceRequest(int resourceId)
    {
        ResourceId = resourceId;
    }
}
