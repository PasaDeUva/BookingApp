namespace BotWhatsapp.Domain.Response;

public class PaginationResponse<T>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int TotalCount { get; set; }
    public List<T> Data { get; set; } = new();
}
