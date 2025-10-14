using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.ToggleBot;

public class ToggleBotCommand : IRequest<ResponseModel<bool>>
{
}
