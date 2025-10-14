namespace BotWhatsapp.Domain.Entities;

public class ReminderRecipient
{
    public int Id { get; set; }
    public int ReminderId { get; set; }
    public Reminder Reminder { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}