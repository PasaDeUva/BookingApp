using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enum;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class CancelarTurnoStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IAppointmentService _appointmentManager;

    public CancelarTurnoStep(IClientSessionRepository clientSessionRepository, IAppointmentService appointmentManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _appointmentManager = appointmentManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        return await ShowAppointmentsAndAskForChoice(session, client);
    }

    private async Task<string> ShowAppointmentsAndAskForChoice(ClientSession session, Domain.Entities.Client client)
    {
        var response = await _appointmentManager.GetByClientIdAsync(client.Id);

        var activeAppointments = response.Data?
            .Where(a => a.Status != AppointmentStatus.Cancelled && a.DateTime >= DateTime.Now)
            .OrderBy(a => a.DateTime)
            .ToList();

        if (activeAppointments == null || !activeAppointments.Any())
        {
            session.CurrentStep = ClientStep.None;
            await _clientSessionRepository.UpdateAsync(session);
            return $"No tienes ningún turno activo para cancelar. Vamos de nuevo... {PromptTemplates.OptionsMenu(client.Name)}";
        }

        var sb = new StringBuilder();
        sb.AppendLine("Estos son tus próximos turnos. Por favor, envía el número del turno que deseas cancelar:");
        sb.AppendLine();

        var appointmentCodes = new List<string>();
        for (int i = 0; i < activeAppointments.Count; i++)
        {
            var appointment = activeAppointments[i];
            sb.AppendLine($"*{i + 1}* - {appointment.Service.Name} el {appointment.DateTime:dd/MM/yyyy} a las {appointment.DateTime:HH:mm} hs.");
            appointmentCodes.Add(appointment.Code);
        }

        session.CurrentStep = ClientStep.WaitingForCancellationChoice;
        await _clientSessionRepository.UpdateAsync(session);

        return sb.ToString();
    }


}
