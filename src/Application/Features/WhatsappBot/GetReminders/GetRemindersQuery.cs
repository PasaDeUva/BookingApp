using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.GetReminders;

public class GetRemindersQuery : IRequest<ResponseModel<List<ReminderResponse>>>
{
}
