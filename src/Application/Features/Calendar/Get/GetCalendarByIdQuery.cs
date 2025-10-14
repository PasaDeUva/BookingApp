using MediatR;
using BotWhatsapp.Domain.Response;
using BotWhatsapp.Domain.Dtos;

namespace BotWhatsapp.Application.Features.Calendar.Get;

public class GetCalendarByIdQuery : IRequest<ResponseModel<CalendarDto>>
{
    public int Id { get; set; }
}
