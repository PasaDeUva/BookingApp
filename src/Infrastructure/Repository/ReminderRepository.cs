using BotWhatsapp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Infrastructure.Repository;

public class ReminderRepository : IReminderRepository
{
    private readonly AppDbContext _context;

    public ReminderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reminder>> GetAllAsync()
    {
        return await _context.Reminders
            .Include(r => r.Recipients)
            .ToListAsync();
    }

    public async Task<Reminder> CreateAsync(Reminder reminder)
    {
        _context.Reminders.Add(reminder);
        await _context.SaveChangesAsync();
        return reminder;
    }

    public async Task<Reminder?> GetByIdAsync(int id)
    {
        return await _context.Reminders
            .Include(r => r.Recipients)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Reminder reminder)

    {
        _context.Reminders.Update(reminder);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var reminder = await _context.Reminders
            .FirstOrDefaultAsync(r => r.Id == id);
        if (reminder != null)
        {
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}