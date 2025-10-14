using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Get;

public class GetCalendarByIdQueryHandler : IRequestHandler<GetCalendarByIdQuery, ResponseModel<CalendarDto>>
{
    private readonly ICalendarService _calendarService;

    public GetCalendarByIdQueryHandler(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<CalendarDto>> Handle(GetCalendarByIdQuery request, CancellationToken cancellationToken)
    {
        return await _calendarService.GetCalendarById(request.Id);
    }
}
