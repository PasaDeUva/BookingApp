using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Interfaces;

public interface IEventRepository
{
    Task<int> CreateAsync(Event entity);
    Task<List<Event>> GetEventsByCode(string code);
}
