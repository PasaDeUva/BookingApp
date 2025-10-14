using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BotWhatsapp.Application.Services;

public class ReminderService : IReminderService
{
    private readonly IReminderRepository _reminderRepository;

    public ReminderService(IReminderRepository reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async Task<List<ReminderDto>> GetAllAsync()
    {
        var reminders = await _reminderRepository.GetAllAsync();
        return reminders.Where(x => x.IsActive).Select(ToDto).ToList();
    }

    public async Task<ReminderDto> CreateAsync(ReminderDto reminderDto)
    {
        if (string.IsNullOrWhiteSpace(reminderDto.Message))
            throw new ArgumentException("El mensaje no puede estar vacío");

        if (!reminderDto.Recipients.Any())
            throw new ArgumentException("Debe especificar al menos un destinatario");

        if (reminderDto.SendDate.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("La fecha de envío no puede ser anterior a hoy");

        var reminder = new Reminder
        {
            Message = reminderDto.Message,
            SendDate = reminderDto.SendDate,
            SendTime = reminderDto.SendTime,
            Recipients = reminderDto.Recipients.Select(r => new ReminderRecipient { PhoneNumber = r }).ToList()
        };

        var created = await _reminderRepository.CreateAsync(reminder);
        return ToDto(created);
    }

    public async Task DeleteAsync(int id)
    {
        var reminder = await _reminderRepository.GetByIdAsync(id);
        if (reminder == null)
            throw new KeyNotFoundException($"Recordatorio con ID {id} no encontrado");

        await _reminderRepository.DeleteAsync(id);
    }

    private static ReminderDto ToDto(Reminder reminder)
    {
        return new ReminderDto
        {
            Id = reminder.Id,
            Message = reminder.Message,
            SendDate = reminder.SendDate,
            SendTime = reminder.SendTime,
            Recipients = reminder.Recipients.Select(r => r.PhoneNumber).ToList(),
            IsSent = reminder.IsSent
        };
    }
}