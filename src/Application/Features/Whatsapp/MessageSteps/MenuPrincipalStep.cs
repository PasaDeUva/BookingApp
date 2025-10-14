using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class MenuPrincipalStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IAppointmentService _appointmentManager;

    public MenuPrincipalStep(IClientSessionRepository clientSessionRepository, IAppointmentService appointmentManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _appointmentManager = appointmentManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        if (message == "1" || message.Trim().ToLower().Contains("crear") || message.Trim().ToLower().Contains("reservar")) // Crear turno
        {
            //session.CurrentStep = ClientStep.AskingDate;
            session.CurrentStep = ClientStep.ChooseDuration;
            await _clientSessionRepository.UpdateAsync(session);
            return $@"¡Perfecto! {PromptTemplates.AnswerDay()}";
        }
        if (message == "2" || message.Trim().ToLower().Contains("reprogramar") || message.Trim().ToLower().Contains("posponer")) // Posponer turno
        {
            session.CurrentStep = ClientStep.RescheduleAppointment;
            await _clientSessionRepository.UpdateAsync(session);

            var appointments = await _appointmentManager.GetByClientIdAsync(client.Id);
            if (!appointments.Success || appointments.Data == null || !appointments.Data.Any())
            {
                session.CurrentStep = ClientStep.MainMenu;
                await _clientSessionRepository.UpdateAsync(session);
                return "No tienes turnos activos para reprogramar. ¿Deseas hacer otra cosa?";
            }

            var appointmentList = string.Join("\n", appointments.Data.Select((a, i) =>
                $@"{ i + 1}. { a.Service.Name} - { a.DateTime:dd / MM / yyyy HH: mm}"));
            return $"Selecciona el turno que deseas reprogramar:\n{appointmentList}";
        }
        if (message == "3" || message.Trim().ToLower().Contains("cancelar")) // Cancelar turno
        {
            session.CurrentStep = ClientStep.CancelAppointment;
            await _clientSessionRepository.UpdateAsync(session);

            var appointments = await _appointmentManager.GetByClientIdAsync(client.Id);
            if (!appointments.Success || appointments.Data == null || !appointments.Data.Any())
            {
                session.CurrentStep = ClientStep.MainMenu;
                await _clientSessionRepository.UpdateAsync(session);
                return "No tienes turnos activos para cancelar. ¿Deseas hacer otra cosa?";
            }

            var appointmentList = string.Join("\n", appointments.Data.Select((a, i) =>
                $@"{ i + 1}. { a.Service.Name} - { a.DateTime:dd / MM / yyyy HH: mm}"));
            return $"Selecciona el turno que deseas cancelar:\n{appointmentList}";
        }
        if (message == "4" || message.Trim().ToLower().Contains("ver")) // Ver turnos
        {
            var appointments = await _appointmentManager.GetByClientIdAsync(client.Id);
            if (!appointments.Success || appointments.Data == null || !appointments.Data.Any())
            {
                return "No tienes turnos activos en este momento.\n\n 🧭 Escribí *volver* para regresar al menú principal.";
            }

            var appointmentList = string.Join("\n", appointments.Data.Select((a, i) =>
                $@"{i + 1}. {a.Service.Name} - {a.DateTime:dd / MM / yyyy HH: mm}"));
            return $"Tus turnos activos son:\n{appointmentList}\n\n 🧭 Escribí *volver* para regresar al menú principal.";
        }
        return @$"Por favor, elige una opción válida del menú. {PromptTemplates.OptionsMenu(client.Name)}";
    }
}
