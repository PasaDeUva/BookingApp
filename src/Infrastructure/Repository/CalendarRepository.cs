using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Repository;

public class CalendarRepository : ICalendarRepository
{
    private readonly AppDbContext _context;

    public CalendarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Calendar>> GetAllAsync()
    {
        return await _context.Calendars.ToListAsync();
    }

    public async Task<Calendar?> GetByIdAsync(int id)
    {
        return await _context.Calendars.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Calendar?> GetByDayAsync(DayOfWeek day)
    {
        return await _context.Calendars.FirstOrDefaultAsync(c => c.Day == day);
    }
    public async Task<IEnumerable<Calendar>> GetByOwnerAsync(int ownerId, CalendarOwnerType ownerType)
    {
        return await _context.Calendars.Where(c => c.OwnerId == ownerId && c.OwnerType == ownerType).ToListAsync();
    }

    public async Task<int> CreateAsync(Calendar calendar)
    {
        _context.Calendars.Add(calendar);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Calendar calendar)
    {
        _context.Entry(calendar).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var calendar = await _context.Calendars.FirstOrDefaultAsync(x => x.Id == id);
        if (calendar == null)
        {
            return 0;
        }
        _context.Calendars.Remove(calendar);
        return await _context.SaveChangesAsync();
    }
}
