using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BotWhatsapp.Application.Services;

public class WhatsappBotService : IWhatsappBotService
{
    private readonly IClientRepository _clientRepository;
    private readonly IEvolutionService _evolutionService;


    private readonly ILogger<WhatsappBotService> _logger;

    public WhatsappBotService(
        IClientRepository clientRepository,
        IEvolutionService evolutionService,
        ILogger<WhatsappBotService> logger)
    {
        _clientRepository = clientRepository;
        _evolutionService = evolutionService;
        _logger = logger;
    }

    public async Task<BotStatus> GetStatusAsync()
    {
        // Implementar lógica para obtener el estado actual del bot
        return new BotStatus
        {
            IsActive = true, // Obtener del estado real del bot
            LastRefresh = DateTime.UtcNow,
            PendingMessages = 0, // Obtener cantidad real de mensajes pendientes
            IsConnected = true // Verificar conexión real del bot
        };
    }

    public async Task ToggleBotAsync(bool active)
    {
        try
        {
            // Implementar lógica para activar/desactivar el bot
            _logger.LogInformation($"Bot status changed to: {active}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling bot status");
            throw new ApplicationException("Error al cambiar el estado del bot", ex);
        }
    }

    public async Task SendMassMessageAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("El mensaje no puede estar vacío");

        var numbersUnbloqued = await _clientRepository.GetUnblockedPhoneNumbers();

        if (numbersUnbloqued.Count < 1)
            throw new ArgumentException("Debe especificar al menos un destinatario");


        foreach (var number in numbersUnbloqued)
        {
            try
            {
                await _evolutionService.SendMessageAsync(number.PhoneNumber, message);
                _logger.LogInformation($"Mensaje enviado a {number}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando mensaje a {number}");
                continue;
            }
        }
    }
}