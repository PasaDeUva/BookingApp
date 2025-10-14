using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class EsperandoNombreStep : IMessageStepStrategy
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IOpenAIService _openAIService;

    public EsperandoNombreStep(IClientRepository clientRepository, IClientSessionRepository clientSessionRepository, IOpenAIService openAIService)
    {
        _clientRepository = clientRepository;
        _clientSessionRepository = clientSessionRepository;
        _openAIService = openAIService;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        var name = await _openAIService.SendToOpenAIAsync($"Trata de extraer el nombre y/o apellido a partir del " +
            $"siguiente mensaje: {message}, recorda devolver el nombre y apellido solo, sin texto que no sirva ejemplo 'Franco Gaido'");

        if (client == null)
        {
            await _clientRepository.CreateAsync(new Domain.Entities.Client { PhoneNumber = session.PhoneNumber, Name = name, IsBloqued = false, IsActive = true });
        }
        else
        {
            client.Name = name;
            await _clientRepository.UpdateAsync(client);
        }

        session.CurrentStep = ClientStep.MainMenu;
        await _clientSessionRepository.UpdateAsync(session);
        return $"✅ Gracias {name}, ahora sí.\n" + PromptTemplates.OptionsMenu(name);
    }
}