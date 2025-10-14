using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Application.EntitiesVM.Appointment;

public class UpdateAppointmentRequest
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public Service? Service { get; set; }
    public int? DurationInMinutes { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; }
    public int? AssignedUserId { get; set; }
}
