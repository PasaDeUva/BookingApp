namespace BotWhatsapp.Domain.EntitiesVM;

public class GetShopsListRequest : PaginationRequest
{
    public int? Id { get; set; }
    public string? Instance { get; set; }
    public string? Name { get; set; }
    public bool? AssistanceEnable { get; set; }
    public bool? IsActive { get; set; }
}
