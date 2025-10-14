using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Get
{
    public class GetCalendarByDayQuery : IRequest<ResponseModel<CalendarDto>>
    {
        public DayOfWeek Day { get; set; }
        public GetCalendarByDayQuery(DayOfWeek day)
        {
            this.Day = day;
        }
    }
}
