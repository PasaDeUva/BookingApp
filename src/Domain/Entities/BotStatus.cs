namespace BotWhatsapp.Domain.Entities;
public class BotStatus
{
    public bool IsActive { get; set; }
    public DateTime LastRefresh { get; set; }
    public int PendingMessages { get; set; }
    public bool IsConnected { get; set; }
}