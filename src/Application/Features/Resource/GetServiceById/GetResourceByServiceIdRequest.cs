using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetServiceById
{
    public class GetResourceByServiceIdRequest : IRequest<ResponseModel<List<ServiceDto>>>
    {
        public int ServiceId { get; set; }

        public GetResourceByServiceIdRequest(int serviceId)
        {
            ServiceId = serviceId;
        }
    }
}
