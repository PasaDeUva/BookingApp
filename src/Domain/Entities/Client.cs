namespace BotWhatsapp.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public bool IsBloqued { get; set; } = false;
}
