using BotWhatsapp.Application.DTOs;

namespace BotWhatsapp.Application.Interfaces
{
    public interface IEvolutionService
    {
        Task SendMessageAsync(string number, string text);
        Task<WhatsAppConnectionDto> GetInstancesAsync();
        Task<Dictionary<string, object>> RestartInstanceAsync();
        Task<WhatsAppDisconnectionDto> DisconectInstanceAsync();
        Task<(string Status, string ExternalReference)> ObtenerEstadoPagoAsync(long paymentId);

    }
}
