using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Get;

public class GetCalendarByDayQueryHandler : IRequestHandler<GetCalendarByDayQuery, ResponseModel<CalendarDto>>
{
    private readonly ICalendarService _calendarService;

    public GetCalendarByDayQueryHandler(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<CalendarDto>> Handle(GetCalendarByDayQuery request, CancellationToken cancellationToken)
    {
        return await _calendarService.GetCalendarByDay(request.Day);
    }
}
