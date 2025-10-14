using BotWhatsapp.Application.Helpers;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Availability;

public class GetAvailabilityHandler : IRequestHandler<GetAvailabilityRequest, List<string>>
{
    private readonly IResourceService _resourceManager;
    private readonly IAppointmentService _appointmentManager;
    private readonly ICalendarService _calendarService;

    public GetAvailabilityHandler(IResourceService resourceManager, IAppointmentService appointmentManager, ICalendarService calendarService)
    {
        _appointmentManager = appointmentManager;
        _calendarService = calendarService;
        _resourceManager = resourceManager;
    }
    public async Task<List<string>> Handle(GetAvailabilityRequest request, CancellationToken cancellationToken)
    {
        // Obtener calendario del recurso
        var calendarResult = await _calendarService.GetAllCalendars();
        if (!calendarResult.Success || calendarResult.Data == null)
            return new List<string>();

        // Filtrar el día de la semana
        var dayOfWeek = request.Date.DayOfWeek;
        var calendarDay = calendarResult.Data.Find(c => c.Day == dayOfWeek && c.IsActive);
        if (calendarDay == null)
            return new List<string>();

        // Obtener turnos agendados
        var appointmentsResult = await _appointmentManager.GetByResourceIdAsync(request.ResourceId);
        var appointments = appointmentsResult.Data?.Where(a => a.DateTime.Date == request.Date.Date).ToList() ?? new List<Appointment>();

        // Calcular intervalos disponibles
        return AvailabilityHelper.GetAvailableSlots(calendarDay, appointments, 0);
    }

}
