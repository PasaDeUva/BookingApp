using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Interfaces;

public interface IClientSessionRepository
{
    Task<ClientSession> GetOrCreateAsync(string phone);
    Task DeleteAsync(string phone);
    Task UpdateAsync(ClientSession session);
    Task UpdateWithStepAsync(ClientSession session, ClientStep? currentStep = null);
}
