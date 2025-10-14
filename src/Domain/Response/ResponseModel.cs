namespace BotWhatsapp.Domain.Response;

public class ResponseModel<T>
{
    public bool Success { get; set; }
    public string? Description { get; set; }
    public T? Data { get; set; }
}