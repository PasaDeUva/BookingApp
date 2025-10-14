using BotWhatsapp.Application.Interfaces;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.RestartInstance;

public class RestartInstanceCommandHandler : IRequestHandler<RestartInstanceCommand, object>
{
    private readonly IEvolutionService _evolutionService;

    public RestartInstanceCommandHandler(IEvolutionService evolutionService)
    {
        _evolutionService = evolutionService;
    }

    public async Task<object> Handle(RestartInstanceCommand request, CancellationToken cancellationToken)
    {
        return await _evolutionService.RestartInstanceAsync();
    }
}
