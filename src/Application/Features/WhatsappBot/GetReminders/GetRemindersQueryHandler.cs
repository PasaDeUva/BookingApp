using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.WhatsappBot.GetReminders;

public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, ResponseModel<List<ReminderResponse>>>
{
    private readonly IWhatsappService _whatsappService;

    public GetRemindersQueryHandler(IWhatsappService whatsappService)
    {
        _whatsappService = whatsappService;
    }

    public async Task<ResponseModel<List<ReminderResponse>>> Handle(GetRemindersQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var reminders = new List<ReminderResponse>();//await _whatsappService.GetRemindersAsync();
            return new ResponseModel<List<ReminderResponse>>
            {
                Data = reminders,
                Description = "Recordatorios obtenidos exitosamente.",
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<List<ReminderResponse>>
            {
                Description = $"Error al obtener recordatorios: {ex.Message}",
                Success = false
            };
        }
    }
}
