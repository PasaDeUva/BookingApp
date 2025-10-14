using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BotWhatsapp.Application.Features.Calendar.Update
{
    public class UpdateCalendarCommandHandler : IRequestHandler<UpdateCalendarCommand, ResponseModel<bool>>
    {
        private readonly ICalendarService _calendarService;

        public UpdateCalendarCommandHandler(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public async Task<ResponseModel<bool>> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
        { 
            return await _calendarService.UpdateCalendar(request.Calendar);
        }
    }
}
