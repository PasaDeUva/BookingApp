namespace BotWhatsapp.Domain.Entities;

public class Shop
{
    public int Id { get; set; }
    public string Instance { get; set; }
    //public string EvolutionApiKey { get; set; }
    //public string OpenAIApiKey { get; set; }
    public string Name { get; set; }
    public bool AssistanceEnable { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}