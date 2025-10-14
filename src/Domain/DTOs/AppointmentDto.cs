using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Dtos;

public class AppointmentDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
    public DateTime DateTime { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ServiceName { get; set; }
    public decimal ServicePrice { get; set; }
    public int ServiceId { get; set; }
    public int? AssignedUserId { get; set; }
    public Resource? AssignedResource { get; set; }
}
