using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enum;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class ReprogramarTurnoStep : IMessageStepStrategy
{
    private readonly IAppointmentService _appointmentManager;
    private readonly IClientSessionRepository _clientSessionRepository;

    public ReprogramarTurnoStep(IAppointmentService appointmentManager, IClientSessionRepository clientSessionRepository)
    {
        _appointmentManager = appointmentManager;
        _clientSessionRepository = clientSessionRepository;
    }

    public async Task<string> ExecuteAsync(ClientSession clientSession, string message, Domain.Entities.Client client)
    {
        var appointments = await _appointmentManager.GetByClientIdAsync(client.Id);
        var appointmentsConfirmed = appointments.Data.Where(a => a.DateTime >= DateTime.Now && a.Status == AppointmentStatus.Confirmed).ToList();
        if (appointmentsConfirmed is null || !appointmentsConfirmed.Any())
        {            
            await _clientSessionRepository.UpdateWithStepAsync(clientSession, ClientStep.MainMenu);
            return $"No tienes turnos activos para reprogramar. Vamos de nuevo... {PromptTemplates.OptionsMenu(client.Name)}";
        }

        var response = new StringBuilder();
        response.AppendLine("Por favor, selecciona el número del turno que deseas reprogramar:");
        response.AppendLine();
        for (int i = 0; i < appointmentsConfirmed.Count; i++)
        {
            var appointment = appointmentsConfirmed[i];
            response.AppendLine($"{i + 1}. Turno para *{appointment.Service.Name}* el día *{appointment.DateTime:dd/MM/yyyy}* a las *{appointment.DateTime:HH:mm}hs* *{appointment.AssignedResource.Name}*.");
        }

        //await _whatsappService.SendMessage(new TextMessage(response.ToString(), message.From));
        await _clientSessionRepository.UpdateWithStepAsync(clientSession, ClientStep.WaitingForRescheduleChoice);
        return response.ToString();
    }
}