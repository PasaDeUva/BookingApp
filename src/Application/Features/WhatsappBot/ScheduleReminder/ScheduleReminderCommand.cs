using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.ScheduleReminder;

public class ScheduleReminderCommand : IRequest<ResponseModel<ReminderResponse>>
{
    public ReminderRequest Request { get; set; }

    public ScheduleReminderCommand(ReminderRequest request)
    {
        Request = request;
    }
}
