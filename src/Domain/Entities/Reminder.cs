namespace BotWhatsapp.Domain.Entities;

public class Reminder
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public TimeSpan SendTime { get; set; }
    public bool IsSent { get; set; }
    public List<ReminderRecipient> Recipients { get; set; } = new();
    public bool IsActive { get; set; } = true;
}