using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Helpers;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Application.Services;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class ElegirDuracionStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IServiceManager _serviceManager;
    private readonly ICalendarService _calendarService;
    private readonly IAppointmentService _appointmentManager;
    private readonly IResourceService _resourceServices;
    private readonly IOpenAIService _openAIService;

    public ElegirDuracionStep(IClientSessionRepository clientSessionRepository, IServiceManager serviceManager, ICalendarService calendarService, 
        IAppointmentService appointmentManager, IResourceService resourceServices, IOpenAIService openAIService)
    {
        _clientSessionRepository = clientSessionRepository;
        _serviceManager = serviceManager;
        _calendarService = calendarService;
        _appointmentManager = appointmentManager;
        _resourceServices = resourceServices;
        _openAIService = openAIService;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        //if (!int.TryParse(message, out int option) || option < 1)
        //{
        //    return "Por favor, selecciona una opción válida.";
        //}

        if (message.Trim().ToLower() == "otros")
        {
            return "Por favor, ingresa la fecha que prefieres en formato 'dd/mm/yyyy', 'mañana', o 'día de la semana'.";
        }

        if (int.TryParse(message, out int dayOption) && dayOption >= 1 && dayOption <= 7)
        {
            DateTime selectedDate = DateTime.Today.AddDays(dayOption - 1);
            session.TempDateRaw = selectedDate.ToString("dd/MM/yyyy");
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
        }

        var request = new ServiceListRequest { PageNumber = 1, PageSize = 100 };
        var res = await _serviceManager.GetAllAsync(request);
        if (!res.Success || res?.Data?.Data.Count == 0)
        {
            session.CurrentStep = ClientStep.None;
            await _clientSessionRepository.UpdateAsync(session);
            return "Lo siento, no hay servicios disponibles en este momento. Por favor, intenta más tarde.";
        }

        var services = res?.Data?.Data;
        //if (option > services?.Count)
        //{
        //    return $"Por favor, selecciona una opción válida (1 a {services?.Count}).";
        //}

        var serviceSelected = services[0];//[option - 1];
        session.ServiceId = serviceSelected.Id;
        session.CurrentStep = ClientStep.AskingTime;
        session.TimeSlotPage = 0; // Inicializar la página en 0

        var availableSlots = await _serviceManager.GetAvailableTimeSlotsWithResourceAsync(session.ServiceId.Value, session.TempDateRaw);

        if (!availableSlots.Any())
        {
            session.CurrentStep = ClientStep.AskingDate;
            await _clientSessionRepository.UpdateAsync(session);
            return "Lo siento, no hay horarios disponibles para esa fecha y duración. Por favor, elige otro día.";
        }

        session.AvailableTimeSlotsJson = JsonSerializer.Serialize(availableSlots);
        await _clientSessionRepository.UpdateAsync(session);

        return ShowTimeSlotPage(availableSlots, 0, serviceSelected.Name, serviceSelected.Price, session.TempDateRaw);
    }
    
    private string ShowTimeSlotPage(List<SlotInfo> allTimeSlots, int page, string serviceName, decimal servicePrice, string date)
    {
        const int slotsPerPage = 10;
        var startIndex = page * slotsPerPage;

        // Parsear el string date a DateTime
        if (!DateTime.TryParse(date, out DateTime selectedDate))
        {
            return "La fecha seleccionada no es válida.";
        }
        if (selectedDate.Date == DateTime.Now.Date)
        {
            allTimeSlots = allTimeSlots
                .Where(s =>
                {
                    // Separar la parte inicial del rango ("13:30" en "13:30-15:00")
                    var startTimeString = s.TimeSlot.Split('-')[0].Trim();

                    // Intentar parsear la hora
                    if (TimeSpan.TryParse(startTimeString, out var startTime))
                    {
                        // Combinar la fecha seleccionada + hora de inicio
                        var slotStartDateTime = selectedDate.Date.Add(startTime);

                        // Comparar contra la hora actual
                        return slotStartDateTime > DateTime.Now;
                    }
                    return false; // Si no se puede parsear, lo descartamos
                })
                .ToList();
        }

        var endIndex = Math.Min(startIndex + slotsPerPage, allTimeSlots.Count);

        if (startIndex >= allTimeSlots.Count)
        {
            return "No hay más horarios disponibles. Escribe 'anterior' para ver horarios previos.";
        }

        var pageSlots = allTimeSlots.Skip(startIndex).Take(slotsPerPage).ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"{serviceName} por {servicePrice:C} para el día {date}.");
        sb.AppendLine();
        sb.AppendLine($"Horarios disponibles:");

        for (int i = 0; i < pageSlots.Count; i++)
        {
            var slot = pageSlots[i];
            sb.AppendLine($"{i + 1}. {slot.TimeSlot} ({slot.ResourceName})");
        }

        sb.AppendLine();
        sb.AppendLine("Escribe el número del horario que prefieres.");

        if (endIndex < allTimeSlots.Count)
        {
            sb.AppendLine("Escribe 'más' para ver más horarios.");
        }
        if (page > 0)
        {
            sb.AppendLine("Escribe 'anterior' para ver horarios previos.");
        }

        return sb.ToString();
    }
        

}
