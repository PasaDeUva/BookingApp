using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Create
{
    public class CreateServiceRequest : IRequest<ResponseModel<bool>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMinutes { get; set; }
        public List<int> ResourceIds { get; set; } = new();
    }
}
