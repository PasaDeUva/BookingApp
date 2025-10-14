using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IWhatsappService
{
    // <summary>
    /// Handles incoming WhatsApp messages for Hairdressed and returns a response. <summary>
    /// Handles incoming WhatsApp messages and returns a response.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    //Task<string> HandleIncomingMessageAsync(int shopId, string phoneNumber, string message);

    // <summary>
    /// Handles incoming WhatsApp messages for Club and returns a response. <summary>
    /// Handles incoming WhatsApp messages and returns a response.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<string> HandleIncomingMessageClubAsync(string phoneNumber, string message);

    //Task<ReminderResponse> ScheduleReminderAsync(ReminderRequest request);
    //Task<List<ReminderResponse>> GetRemindersAsync();
}
