namespace BotWhatsapp.Domain.Dtos;
public class ResourceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public List<int> ServiceIds { get; set; }
    public List<CalendarDto> Calendars { get; set; }
}