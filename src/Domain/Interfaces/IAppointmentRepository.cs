using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM.Appointment;

namespace BotWhatsapp.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Tuple<List<Appointment>, int>> GetAllAsync(AppointmentFilters request);
    Task<int> CreateAsync(Appointment appointment);
    Task<int> UpdateAsync(Appointment appointment);
    Task<int> CancelAsync(int appointmentId);
    Task<Appointment?> GetByIdAsync(int id);
    Task<List<Appointment>> GetByClientIdAsync(int clientId);
    Task<List<Appointment>> GetUpcomingByClientIdAsync(int clientId);
    Task<Appointment?> GetNextAppointmentByClientIdAsync(int clientId);
    Task UpdateDateAsync(int appointmentId, DateTime newDate);
    Task<List<Appointment>> GetByResourceIdAsync(int resourceId);
    Task<List<Appointment>> GetByResourceIdBetweenDatesAsync(int resourceId, DateTime from, DateTime to);
    Task<bool> CodeExistsAsync(string code);
    Task<Appointment?> GetByCodeAsync(int id);
    Task<List<Appointment>> GetByPhoneNumberAsync(string phoneNumber);
}
