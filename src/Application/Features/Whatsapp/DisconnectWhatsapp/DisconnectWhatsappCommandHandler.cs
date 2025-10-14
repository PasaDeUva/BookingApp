using BotWhatsapp.Application.Interfaces;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.DisconnectWhatsapp;

public class DisconnectWhatsappCommandHandler : IRequestHandler<DisconnectWhatsappCommand, string>
{
    private readonly IEvolutionService _evolutionService;

    public DisconnectWhatsappCommandHandler(IEvolutionService evolutionService)
    {
        _evolutionService = evolutionService;
    }

    public async Task<string> Handle(DisconnectWhatsappCommand request, CancellationToken cancellationToken)
    {
        await _evolutionService.DisconectInstanceAsync();
        return "WhatsApp desvinculado exitosamente";
    }
}
