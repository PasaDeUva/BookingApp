using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Dtos;
public class CalendarDto
{
    public int Id { get; set; }    
    public DayOfWeek Day { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public CalendarOwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }
}