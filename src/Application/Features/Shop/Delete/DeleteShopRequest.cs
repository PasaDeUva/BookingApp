using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.Delete
{
    public class DeleteShopRequest : IRequest<ResponseModel<bool>>
    {
        public int Id { get; set; }
        public DeleteShopRequest(int id)
        {
            Id = id;
        }
    }
}
