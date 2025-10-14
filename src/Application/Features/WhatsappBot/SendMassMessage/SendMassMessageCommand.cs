using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.SendMassMessage;

public class SendMassMessageCommand : IRequest<ResponseModel<bool>>
{
    public MassMessageRequest Request { get; set; }

    public SendMassMessageCommand(MassMessageRequest request)
    {
        Request = request;
    }
}
