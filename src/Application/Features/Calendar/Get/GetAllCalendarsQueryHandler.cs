using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Get;

public class GetAllCalendarsQueryHandler : IRequestHandler<GetAllCalendarsQuery, ResponseModel<List<CalendarDto>>>
{
    private readonly ICalendarService _calendarService;

    public GetAllCalendarsQueryHandler(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<List<CalendarDto>>> Handle(GetAllCalendarsQuery request, CancellationToken cancellationToken)
    {
        return await _calendarService.GetAllCalendars();
    }
}
