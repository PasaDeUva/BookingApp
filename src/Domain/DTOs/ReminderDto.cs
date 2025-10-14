namespace BotWhatsapp.Domain.Dtos;
public class ReminderDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public TimeSpan SendTime { get; set; }
    public List<string> Recipients { get; set; } = new();
    public bool IsSent { get; set; }
}