using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Interfaces;

public interface IReminderRepository
{
    Task<List<Reminder>> GetAllAsync();
    Task<Reminder> CreateAsync(Reminder reminder);
    Task<Reminder?> GetByIdAsync(int id);
    Task UpdateAsync(Reminder reminder);
    Task DeleteAsync(int id);
}