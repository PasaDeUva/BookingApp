using MediatR;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Features.Calendar.Delete;

public class DeleteCalendarCommand : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
}
