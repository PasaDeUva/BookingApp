using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class ElegirDiaStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IServiceManager _serviceManager;

    public ElegirDiaStep(IClientSessionRepository clientSessionRepository, IOpenAIService openAIService, IServiceManager serviceManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _openAIService = openAIService;
        _serviceManager = serviceManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        if (message.Trim().ToLower() == "otros")
        {
            return "Por favor, ingresa la fecha que prefieres en formato 'dd/mm/yyyy', 'mañana', o 'día de la semana'.";
        }

        if (int.TryParse(message, out int dayOption) && dayOption >= 1 && dayOption <= 7)
        {
            DateTime selectedDate = DateTime.Today.AddDays(dayOption - 1);
            session.TempDateRaw = selectedDate.ToString("dd/MM/yyyy");
            session.CurrentStep = ClientStep.ChooseDuration;
            await _clientSessionRepository.UpdateAsync(session);
            return await ShowDurationOptions();
        }
        else
        {
            var dateResponse = await _openAIService.SendToOpenAIAsync(PromptTemplates.DateExtraction(message));
            if (dateResponse == "INVALID")
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                var dayAfterTomorrow = today.AddDays(2);
                var threeDaysLater = today.AddDays(3);
                var fourDaysLater = today.AddDays(4);
                var fiveDaysLater = today.AddDays(5);
                var sixDaysLater = today.AddDays(6);
                var sevenDaysLater = today.AddDays(7);

                return $@"
                            No pude entender la fecha. Por favor, elige una de estas opciones:
                            1️⃣ {today:dddd, dd/MM} (hoy)
                            2️⃣ {tomorrow:dddd, dd/MM} (mañana)
                            3️⃣ {dayAfterTomorrow:dddd, dd/MM}
                            4️⃣ {threeDaysLater:dddd, dd/MM}
                            5️⃣ {fourDaysLater:dddd, dd/MM}
                            6️⃣ {fiveDaysLater:dddd, dd/MM}
                            7️⃣ {sixDaysLater:dddd, dd/MM}
                            O escribe 'otros' para ingresar otra fecha.";
            }

            session.TempDateRaw = message;
            session.CurrentStep = ClientStep.ChooseDuration;
            await _clientSessionRepository.UpdateAsync(session);
            return await ShowDurationOptions();
        }
    }

    private async Task<string> ShowDurationOptions()
    {
        var request = new ServiceListRequest { PageNumber = 1, PageSize = 100, };
        var res = await _serviceManager.GetAllAsync(request);
        if (!res.Success || res?.Data?.Data.Count == 0)
        {
            return "Lo siento, no hay servicios disponibles en este momento. Por favor, intenta más tarde.";
        }

        var message = "¿Cuánto tiempo deseas reservar la cancha?\n";
        var services = res?.Data?.Data;

        for (int i = 0; i < services.Count; i++)
        {
            var servicio = services[i];
            message += $"{i + 1}. {servicio.Name} (${servicio.Price:N2})\n";
        }

        message += "Escribe el número de la opción.";
        return message;
    }
}
