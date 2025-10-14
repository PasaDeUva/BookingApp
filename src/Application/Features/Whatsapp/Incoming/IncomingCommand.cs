using BotWhatsapp.Domain.Dtos;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.Incoming;

public class IncomingCommand : IRequest<string>
{
    public IncomingWebhookDto Dto { get; set; }

    public IncomingCommand(IncomingWebhookDto dto)
    {
        Dto = dto;
    }
}
