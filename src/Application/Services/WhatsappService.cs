using BotWhatsapp.Application.Features.Whatsapp;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text.Json;
using System.Xml.Linq;

namespace BotWhatsapp.Application.Services;

public class WhatsappService : IWhatsappService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IOpenAIService _openAIService;
    private readonly ISessionService _sessionManager;
    private readonly MessageStepResolver _messageStepResolver;

    public WhatsappService(IClientRepository clientRepository, IClientSessionRepository clientSessionRepository, IOpenAIService openAIService, ISessionService sessionManager, MessageStepResolver messageStepResolver)
    {
        _clientRepository = clientRepository;
        _clientSessionRepository = clientSessionRepository;
        _openAIService = openAIService;
        _sessionManager = sessionManager;
        _messageStepResolver = messageStepResolver;
    }

    public async Task<string> HandleIncomingMessageClubAsync(string phoneNumber, string message)
    {
        phoneNumber = phoneNumber.Split('@')[0];
        var client = await _clientRepository.GetByPhoneNumberAsync(phoneNumber);
        var session = await _clientSessionRepository.GetOrCreateAsync(phoneNumber);

        var response = await _openAIService.SendToOpenAIAsync(PromptTemplates.ClassifyGreeting(message));
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<GreetingResult>(response, options);

        var hasName = client != null && !string.IsNullOrEmpty(client?.Name);

        switch (result?.Classification)
        {
            case "InitialGreeting":
            case var _ when message.Trim().ToLower() == "volver":
                if (!hasName)
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.MainMenu);
                    return "👋 ¡Hola! ¿Cómo te llamás?";
                }
                else
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                    return PromptTemplates.OptionsMenu(client?.Name);
                }
            case "CourtesyGreeting":
                if (!hasName)
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.MainMenu);
                    return "😃 ¡Todo bien por acá! Gracias por preguntar. ¿Me indicarías tu nombre para poder continuar?";
                }
                else
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                    return $"🙌 ¡Todo en orden! ¡Avancemos con las opciones disponibles! {PromptTemplates.OptionsMenu(client.Name)}";
                }

            case "Other":
                if (!hasName && !string.IsNullOrEmpty(result.Name))
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                    await _clientRepository.CreateAsync(new Client { PhoneNumber = phoneNumber, Name = result.Name });
                    await _clientSessionRepository.DeleteAsync(phoneNumber);
                    return $"✅ Gracias {result.Name}, ahora sí.\n" + PromptTemplates.ShowMainMenu();
                }
                else if (!hasName && string.IsNullOrEmpty(result.Name))
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.MainMenu);
                    return await _openAIService.SendToOpenAIAsync(PromptTemplates.ConversationalPrompt(message));
                    //return "😃 ¡Avancemos! ¿Me indicarías tu nombre para poder continuar?";
                }
                break;
        }

        if (client == null || string.IsNullOrEmpty(client.Name))
        {
            return await _sessionManager.HandleNameRequestAsync(phoneNumber, message, session);
        }

        if (string.IsNullOrEmpty(session.CurrentStep.ToString()) || session.CurrentStep == ClientStep.None)
        {            
            if (message == "2")
            {
                session.CurrentStep = ClientStep.RescheduleAppointment;
                await _clientSessionRepository.UpdateAsync(session);
            }
            else if (message == "3")
            {
                session.CurrentStep = ClientStep.CancelAppointment;
                await _clientSessionRepository.UpdateAsync(session);
            }
            else if (message == "4")
            {
                session.CurrentStep = ClientStep.ViewAppointment;
                await _clientSessionRepository.UpdateAsync(session);
            }
            else
            {
                session.CurrentStep = ClientStep.MainMenu;
                await _clientSessionRepository.UpdateAsync(session);
            }
        }

        var strategy = _messageStepResolver.Resolve(session.CurrentStep);
        if (strategy != null)
        {
            return await strategy.ExecuteAsync(session, message, client);
        }

        session.CurrentStep = ClientStep.MainMenu;
        await _clientSessionRepository.UpdateAsync(session);
        return PromptTemplates.OptionsMenu(client.Name);
    }
}