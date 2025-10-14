
using BotWhatsapp.Domain.Enum;

namespace BotWhatsapp.Domain.EntitiesVM.Appointment;
public class UpdateAppointmentRequest : CreateAppointmentRequest
{
    public int Id { get; set; }
    public AppointmentStatus Status { get; set; }
}