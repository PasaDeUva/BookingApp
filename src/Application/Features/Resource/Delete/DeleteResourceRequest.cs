using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Delete;

public class DeleteResourceRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    public DeleteResourceRequest(int id)
    {
        Id = id;
    }
}
