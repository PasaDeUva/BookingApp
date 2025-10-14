using BotWhatsapp.Domain.Enums; // Added for TimeSpan
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Create;

public class CreateCalendarCommand : IRequest<ResponseModel<bool>>
{
    public List<DayOfWeek> Days { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int OwnerId { get; set; }
    public CalendarOwnerType OwnerType { get; set; }
}
