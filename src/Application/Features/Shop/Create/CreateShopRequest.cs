using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.Create
{
    public class CreateShopRequest : IRequest<ResponseModel<bool>>
    {
        public string Instance { get; set; }
        public string Name { get; set; }
        public bool AssistanceEnable { get; set; }
        public bool IsActive { get; set; }
    }
}
