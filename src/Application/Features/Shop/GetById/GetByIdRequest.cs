using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Shop.GetById
{
    public class GetByIdRequest : IRequest<ResponseModel<ShopDto>>
    {
        public int Id { get; set; }
        public GetByIdRequest(int id)
        {
            Id = id;
        }
    }
}
