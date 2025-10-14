using BotWhatsapp.Domain.Enum;

namespace BotWhatsapp.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public DateTime DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int? AssignedResourceId { get; set; }
    public Resource? AssignedResource { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public bool IsActive { get; set; } = true;
}


