namespace BotWhatsapp.Domain.EntitiesVM;
public class ReminderRequest
{
    public string Message { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public TimeSpan SendTime { get; set; }
    public List<string> Recipients { get; set; } = new();
}