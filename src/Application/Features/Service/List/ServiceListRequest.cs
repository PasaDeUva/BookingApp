using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.List
{
    public class ServiceListRequest : PaginationRequest , IRequest<ResponseModel<PaginationResponse<ServiceDto>>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? DurationInMinutes { get; set; }
    }

}
