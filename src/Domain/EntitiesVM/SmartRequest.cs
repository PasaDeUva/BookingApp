namespace BotWhatsapp.Domain.EntitiesVM;

public class SmartRequest
{
    public string ClientName { get; set; } = "";
    public string Context { get; set; } = "";
    public string Message { get; set; } = "";
    public string Phone { get; set; } = "";
    public AppointmentInfo? UpcomingAppointment { get; set; }
}

public class AppointmentInfo
{
    public DateTime DateTime { get; set; }
    public string Status { get; set; } = "";
}
