namespace BotWhatsapp.Domain.Entities;

public class CalendarIntervals
{
    public int Id { get; set; }
    public int CalendarId { get; set; }
    
    public bool IsActive { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
