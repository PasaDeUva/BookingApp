namespace BotWhatsapp.Domain.EntitiesVM;

public class ClientCreateRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class ClientUpdateRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class ClientBlockRequest
{
    public bool isBloqued { get; set; }
}
