using BotWhatsapp.Domain.Dtos;

namespace BotWhatsapp.Application.Interfaces;

public interface IReminderService
{
    Task<List<ReminderDto>> GetAllAsync();
    Task<ReminderDto> CreateAsync(ReminderDto reminderDto);
    Task DeleteAsync(int id);
}