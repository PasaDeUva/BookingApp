using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Delete
{
    public class DeleteServiceRequest : IRequest<ResponseModel<bool>>
    {
        public int Id { get; set; }
        public DeleteServiceRequest(int id)
        {
            Id = id;
        }
    }
}
