using BotWhatsapp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Infrastructure.Repository;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Event entity)
    {
        try
        {
            _context.Events.Add(entity);
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<List<Event>> GetEventsByCode(string code)
    {
        return await _context.Events.Where(e => e.AppointmentCode == code).ToListAsync();
    }
}
