using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetById;

public class GetByIdResourceRequest : IRequest<ResponseModel<ResourceDto>>
{
    public int Id { get; set; }
    public GetByIdResourceRequest(int id)
    {
        Id = id;
    }
}
