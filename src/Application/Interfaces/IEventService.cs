using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IEventService
{
    /// <summary>
    /// Creates a new event.
    /// </summary>
    /// <param name="entity">The event entity to create.</param>
    /// <returns>The number of rows affected.</returns>
    Task<ResponseModel<bool>> CreateAsync(Event entity);
    /// <summary>
    /// Retrieves a list of events by appointment code.
    /// </summary>
    /// <param name="code">The appointment code to filter events.</param>
    /// <returns>A list of events matching the specified code.</returns>
    Task<ResponseModel<List<Event>>> GetEventsByCode(string code);
}
