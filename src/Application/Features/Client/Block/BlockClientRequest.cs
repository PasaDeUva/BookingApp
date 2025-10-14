using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Block;

public class BlockClientRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    public bool IsBloqued { get; set; }
    
    public BlockClientRequest(int id, bool isBloqued)
    {
        Id = id;
        IsBloqued = isBloqued;
    }
}