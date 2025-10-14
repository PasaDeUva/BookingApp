using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enum;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text.Json.Nodes;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class WaitingForRescheduleChoiceStep : IMessageStepStrategy
{
    private readonly IAppointmentService _appointmentManager;
    private readonly IClientSessionRepository _clientSessionRepository;

    public WaitingForRescheduleChoiceStep(IAppointmentService appointmentManager, IClientSessionRepository clientSessionRepository)
    {
        _appointmentManager = appointmentManager;
        _clientSessionRepository = clientSessionRepository;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        if (!int.TryParse(message, out int choice))
        {
            return "Por favor, ingresa un número válido.";
        }

        var appointments = await _appointmentManager.GetByPhoneNumberAsync(client.PhoneNumber);
        var appointmentsConfirmed = appointments.Data?.Where(a => a.DateTime >= DateTime.Now && a.Status == AppointmentStatus.Confirmed).ToList();

        if (appointmentsConfirmed is null || choice <= 0 || choice > appointmentsConfirmed.Count)
        {
            return "La opción que elegiste no es válida. Por favor, intenta de nuevo.";
        }

        var appointmentToReschedule = appointmentsConfirmed[choice - 1];

        session.AppointmentToReescheduleJson = new JsonObject { { "AppointmentIdToReschedule", appointmentToReschedule.Id } }.ToString();

        await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.ChooseDuration);
        return $@"¡Perfecto! Ahora procedamos a agendar tu nuevo turno. {PromptTemplates.AnswerDay()}";
    }
}
