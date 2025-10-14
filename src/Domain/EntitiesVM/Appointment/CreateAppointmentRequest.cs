namespace BotWhatsapp.Domain.EntitiesVM.Appointment;

public class CreateAppointmentRequest
{
    public int ClientId { get; set; }
    public int? DurationInMinutes { get; set; }
    public int? AssignedResourceId { get; set; }
    public int ServiceId { get; set; }
    public DateTime DateTime { get; set; }

}
