using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public List<int> AssignedResourceIds { get; set; } = new();
}
