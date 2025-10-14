using BotWhatsapp.Domain.Enum;

namespace BotWhatsapp.Domain.EntitiesVM.Appointment;
public class AppointmentFilters : PaginationRequest
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? ResourceId { get; set; }
    public AppointmentStatus? Status { get; set; }
    public string? ClientName { get; set; }
}