namespace BotWhatsapp.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    
    public string AppointmentCode { get; set; }
    public DateTime DateEvent { get; set; }
    public string Description { get; set; }
}
