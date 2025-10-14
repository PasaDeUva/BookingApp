using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetResource
{
    public class ResourceListRequest : PaginationRequest, IRequest<ResponseModel<PaginationResponse<ResourceDto>>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
