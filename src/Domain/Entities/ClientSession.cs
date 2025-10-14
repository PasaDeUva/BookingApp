using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Entities;

public class ClientSession
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Intent { get; set; }
    public ClientStep CurrentStep { get; set; }
    public DateTime? TempDate { get; set; }
    public int? AppointmentId { get; set; }
    public string? TempDateRaw { get; set; }
    public string? TempTimeRaw { get; set; }
    public int? ServiceId { get; set; }
    public int? AssignedResourceId { get; set; }
    public int TimeSlotPage { get; set; } = 0;
    public string? AvailableTimeSlotsJson { get; set; }
    public bool IsActive { get; set; } = true;
    public string? AppointmentToReescheduleJson { get; set; }

}
