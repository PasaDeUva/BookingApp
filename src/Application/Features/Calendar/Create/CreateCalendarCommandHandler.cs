using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks; // Added for Task
using System;

namespace BotWhatsapp.Application.Features.Calendar.Create;

public class CreateCalendarCommandHandler : IRequestHandler<CreateCalendarCommand, ResponseModel<bool>>
{
    private readonly ICalendarService _calendarService;

    public CreateCalendarCommandHandler(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<bool>> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
    {
        if (!TimeSpan.TryParse(request.StartTime, out var startTime) || !TimeSpan.TryParse(request.EndTime, out var endTime))
        {
            return new ResponseModel<bool> { Success = false, Description = "Invalid time format for StartTime or EndTime." };
        }

        return await _calendarService.CreateCalendar(request.Days, startTime, endTime, request.OwnerId, request.OwnerType);
    }
}
