using MediatR;
using BotWhatsapp.Domain.Response;
using BotWhatsapp.Domain.Dtos;
using System.Collections.Generic;

namespace BotWhatsapp.Application.Features.Calendar.Get
{
    public class GetAllCalendarsQuery : IRequest<ResponseModel<List<CalendarDto>>>
    {
    }
}
