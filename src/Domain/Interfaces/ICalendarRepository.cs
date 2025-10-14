using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotWhatsapp.Domain.Interfaces;

public interface ICalendarRepository
{
    Task<List<Calendar>> GetAllAsync();
    Task<Calendar?> GetByIdAsync(int id);
    Task<Calendar?> GetByDayAsync(DayOfWeek day);
    Task<IEnumerable<Calendar>> GetByOwnerAsync(int ownerId, CalendarOwnerType ownerType);
    Task<int> CreateAsync(Calendar calendar);
    Task<int> UpdateAsync(Calendar calendar);
    Task<int> DeleteAsync(int id);
}