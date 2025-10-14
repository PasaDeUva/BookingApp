using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Response;
using System.Collections.Generic; // Added for List<string>
using System; // Added for TimeSpan

namespace BotWhatsapp.Application.Interfaces;

public interface ICalendarService
{
    Task<ResponseModel<bool>> CreateCalendar(List<DayOfWeek> days, TimeSpan startTime, TimeSpan endTime, int ownerId, CalendarOwnerType ownerType);
    Task<ResponseModel<bool>> UpdateCalendar(CalendarDto calendarDto);
    Task<ResponseModel<List<CalendarDto>>> GetAllCalendars();
    Task<ResponseModel<CalendarDto>> GetCalendarByDay(DayOfWeek day);
    Task<ResponseModel<CalendarDto>> GetCalendarById(int id);
    Task<ResponseModel<bool>> DeleteCalendar(int id);
}
