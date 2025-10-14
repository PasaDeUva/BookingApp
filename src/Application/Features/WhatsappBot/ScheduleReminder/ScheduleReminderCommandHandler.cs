using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.ScheduleReminder;

public class ScheduleReminderCommandHandler : IRequestHandler<ScheduleReminderCommand, ResponseModel<ReminderResponse>>
{
    private readonly IWhatsappService _whatsappService;

    public ScheduleReminderCommandHandler(IWhatsappService whatsappService)
    {
        _whatsappService = whatsappService;
    }

    public async Task<ResponseModel<ReminderResponse>> Handle(ScheduleReminderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var reminder = new ReminderResponse();//await _whatsappService.ScheduleReminderAsync(command.Request);
            return new ResponseModel<ReminderResponse> { Data = reminder, Success = true, Description = "Recordatorios creados exitosamente." };
        }
        catch (Exception ex)
        {
            return new ResponseModel<ReminderResponse> { Success = false, Description = $"Error al programar recordatorio: {ex.Message}" };
        }
    }
}
