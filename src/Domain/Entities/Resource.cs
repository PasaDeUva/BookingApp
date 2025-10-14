namespace BotWhatsapp.Domain.Entities;

public class Resource
{
    public int Id { get; set; }    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;
}