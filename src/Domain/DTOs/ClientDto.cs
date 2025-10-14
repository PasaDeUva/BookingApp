namespace BotWhatsapp.Domain.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public bool IsBloqued { get; set; } = false;
    
    public ShopDto? Shop { get; set; }
}
