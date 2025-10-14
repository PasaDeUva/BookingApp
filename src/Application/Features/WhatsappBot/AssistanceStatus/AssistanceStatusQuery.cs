using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.AssistanceStatus;

public class AssistanceStatusQuery : IRequest<ResponseModel<bool>>
{
}
