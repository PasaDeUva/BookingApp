namespace BotWhatsapp.Domain.Entities;

public class BlockedNumber
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    public Shop Shop { get; set; }
}