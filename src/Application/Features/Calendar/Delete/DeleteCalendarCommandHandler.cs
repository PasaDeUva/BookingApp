using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Delete;

public class DeleteCalendarCommandHandler : IRequestHandler<DeleteCalendarCommand, ResponseModel<bool>>
{
    private readonly ICalendarService _calendarService;

    public DeleteCalendarCommandHandler(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<bool>> Handle(DeleteCalendarCommand request, CancellationToken cancellationToken)
    {
        return await _calendarService.DeleteCalendar(request.Id);
    }
}
