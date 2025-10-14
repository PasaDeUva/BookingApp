namespace BotWhatsapp.Domain.Entities;

public class ResourceService
{
    public int Id { get; set; }
    
    public int ResourceId { get; set; }
    public Resource Resource { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public bool IsActive { get; set; } = true;
}