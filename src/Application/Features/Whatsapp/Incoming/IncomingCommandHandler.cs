using BotWhatsapp.Application.Interfaces;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.Incoming;

public class IncomingCommandHandler : IRequestHandler<IncomingCommand, string>
{
    private readonly IWhatsappService _whatsappService;
    private readonly IEvolutionService _evolutionService;

    public IncomingCommandHandler(IWhatsappService whatsappService, IEvolutionService evolutionService)
    {
        _whatsappService = whatsappService;
        _evolutionService = evolutionService;
    }

    public async Task<string> Handle(IncomingCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Dto.Data?.Message?.Conversation))
        {
            var responseText = await _whatsappService.HandleIncomingMessageClubAsync(request.Dto.Data.Key.RemoteJid, request.Dto.Data?.Message?.Conversation ?? "");

            if (!string.IsNullOrEmpty(responseText))
            {
                await _evolutionService.SendMessageAsync(request.Dto.Data.Key.RemoteJid, responseText);

                return responseText;
            }
        }

        return "✅ Mensaje procesado pero vacio.";
    }
}
