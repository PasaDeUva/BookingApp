using BotWhatsapp.Application.Interfaces;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.GetQR;

public class GetQRQueryHandler : IRequestHandler<GetQRQuery, object>
{
    private readonly IEvolutionService _evolutionService;

    public GetQRQueryHandler(IEvolutionService evolutionService)
    {
        _evolutionService = evolutionService;
    }

    public async Task<object> Handle(GetQRQuery request, CancellationToken cancellationToken)
    {
        return await _evolutionService.GetInstancesAsync();
    }
}
