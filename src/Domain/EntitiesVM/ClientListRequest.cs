namespace BotWhatsapp.Domain.EntitiesVM;

public class ClientListRequest : PaginationRequest
{
    
    public int? Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsBloqued { get; set; }
}