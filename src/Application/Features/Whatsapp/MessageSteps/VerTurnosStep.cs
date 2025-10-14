using System.Text;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class VerTurnosStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IAppointmentService _appointmentManager;

    public VerTurnosStep(IClientSessionRepository clientSessionRepository, IAppointmentService appointmentManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _appointmentManager = appointmentManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        var response = await _appointmentManager.GetByClientIdAsync(client.Id);

        if (!response.Success || response.Data == null || !response.Data.Any())
        {
            session.CurrentStep = ClientStep.None;
            await _clientSessionRepository.UpdateAsync(session);
            return $@"No tienes ningún turno reservado por el momento. Vamos de nuevo... {Environment.NewLine}{PromptTemplates.OptionsMenu(client.Name)} ";
        }

        var activeAppointments = response.Data
            .Where(a => a.Status != Domain.Enum.AppointmentStatus.Cancelled && a.DateTime >= DateTime.Now)
            .OrderBy(a => a.DateTime)
            .ToList();

        if (!activeAppointments.Any())
        {
            session.CurrentStep = ClientStep.None;
            await _clientSessionRepository.UpdateAsync(session);
            return $@"No tienes ningún turno activo por el momento. Vamos de nuevo... {Environment.NewLine}{PromptTemplates.OptionsMenu(client.Name)} ";
        }

        var sb = new StringBuilder();
        sb.AppendLine("Aquí tienes tus próximos turnos:");
        sb.AppendLine();

        foreach (var appointment in activeAppointments)
        {
            sb.AppendLine($"*Servicio:* {appointment.Service.Name}");
            sb.AppendLine($"*Fecha:* {appointment.DateTime:dd/MM/yyyy}");
            sb.AppendLine($"*Hora:* {appointment.DateTime:HH:mm} hs ({appointment.AssignedResource.Name})");
            sb.AppendLine($"*Código de turno:* {appointment.Code}");
            sb.AppendLine("--------------------");
        }
        sb.AppendLine();
        sb.AppendLine($"🧭 Escribí *volver* para regresar al menú principal.");

        session.CurrentStep = ClientStep.None;
        await _clientSessionRepository.UpdateAsync(session);

        return sb.ToString();
    }
}
