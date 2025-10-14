using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.EntitiesVM;

public class CalendarVM
{
    public List<DayOfWeek> Days { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public CalendarOwnerType OwnerType { get; set; } // Shop o Resource
    public int OwnerId { get; set; } // Id del shop o del recurso

}
