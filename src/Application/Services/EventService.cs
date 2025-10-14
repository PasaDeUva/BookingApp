using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<ResponseModel<bool>> CreateAsync(Event entity)
    {
        try
        {
            entity.DateEvent = DateTime.UtcNow;
            var result = await _eventRepository.CreateAsync(entity);
            return new ResponseModel<bool>
            {
                Data = result > 0,
                Success = result > 0,
                Description = result > 0 ? "Event created successfully." : "Failed to create event."
            };
        }
        catch (Exception ex)
        {
            entity.DateEvent = DateTime.UtcNow;
            entity.Description = ex.Message;
            await _eventRepository.CreateAsync(entity);
            return new ResponseModel<bool>
            {
                Data = false,
                Success = false,
                Description = "An error occurred while creating the event: " + ex.Message
            };
        }
    }

    public async Task<ResponseModel<List<Event>>> GetEventsByCode(string code)
    {
        try
        {
            var events = await _eventRepository.GetEventsByCode(code);
            return new ResponseModel<List<Event>>
            {
                Data = events,
                Success = true,
                Description = "Events retrieved successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<List<Event>>
            {
                Data = null,
                Success = false,
                Description = "An error occurred while retrieving events: " + ex.Message
            };
        }
    }
}
