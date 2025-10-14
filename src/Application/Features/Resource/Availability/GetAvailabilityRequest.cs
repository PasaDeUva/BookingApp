using MediatR;
namespace BotWhatsapp.Application.Features.Resource.Availability;

public class GetAvailabilityRequest : IRequest<List<string>>
{
    public int ResourceId { get; set; }
    public DateTime Date { get; set; }

    public GetAvailabilityRequest(int resourceId, DateTime date)
    {
        ResourceId = resourceId;
        Date = date;
    }
}
