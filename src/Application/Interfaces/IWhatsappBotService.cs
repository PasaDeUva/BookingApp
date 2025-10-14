using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Application.Interfaces;

public interface IWhatsappBotService
{
    Task<BotStatus> GetStatusAsync();
    Task ToggleBotAsync(bool active);
    Task SendMassMessageAsync(string message);
}