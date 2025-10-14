using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Delete;

public class DeleteClientRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    
    public DeleteClientRequest(int id)
    {
        Id = id;
    }
}