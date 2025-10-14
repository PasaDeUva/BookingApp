namespace BotWhatsapp.Application.Interfaces
{
    public interface IOpenAIService
    {
        Task<DateTime?> ExtractDateFromText(string message);
        Task<string> SendToOpenAIAsync(string prompt);
    }
}
