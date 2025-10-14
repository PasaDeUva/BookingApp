using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Update;

public class UpdateServiceRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public List<int> ResourceIds { get; set; } = new();
}
