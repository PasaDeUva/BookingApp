using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.GetById;

public class GetServiceByIdRequest : IRequest<ResponseModel<ServiceDto>>
{
    public int Id { get; set; }
    public GetServiceByIdRequest(int id)
    {
        Id = id;
    }
}
