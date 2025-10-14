using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM.Appointment;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IAppointmentService
{
    Task<ResponseModel<PaginationResponse<Appointment>>> GetAllAsync(AppointmentFilters filters);
    Task<ResponseModel<bool>> CreateAsync(CreateAppointmentRequest request);
    Task<ResponseModel<bool>> UpdateAsync(UpdateAppointmentRequest request);
    Task<ResponseModel<bool>> UpdateByDateAsync(UpdateAppointmentByDateRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<Appointment>> GetByIdAsync(int id);
    Task<ResponseModel<List<Appointment>>> GetByClientIdAsync(int clientId);
    Task<ResponseModel<Appointment>> GetByCodeAsync(int id);
    Task<ResponseModel<bool>> IsResourceAvailable(int employeeId, DateTime dateTime, int duration);
    Task<ResponseModel<List<Appointment>>> GetByResourceIdAsync(int resourceId);
    Task<ResponseModel<List<Appointment>>> GetByPhoneNumberAsync(string phoneNumber);
}
