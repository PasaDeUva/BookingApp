using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Calendar.Update
{
    public class UpdateCalendarCommand : IRequest<ResponseModel<bool>>
    {
        public CalendarDto Calendar { get; set; }
    }
}
