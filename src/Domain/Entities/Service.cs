namespace BotWhatsapp.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public ICollection<ResourceService> ResourceServices { get; set; } = new List<ResourceService>();
    public bool IsActive { get; set; } = true;
}
