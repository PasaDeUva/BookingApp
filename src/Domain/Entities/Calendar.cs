using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Entities;

public class Calendar
{
    public int Id { get; set; }    
    public DayOfWeek Day { get; set; }
    public bool IsActive { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public CalendarOwnerType OwnerType { get; set; } // Shop o Resource
    public int OwnerId { get; set; } // Id del shop o del recurso
}

