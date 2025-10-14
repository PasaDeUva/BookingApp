using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Application.Prompts;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM.Appointment;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Text.Json;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class ConfirmarTurnoStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IAppointmentService _appointmentManager;
    private readonly IOpenAIService _openAIService;

    public ConfirmarTurnoStep(IClientSessionRepository clientSessionRepository, IAppointmentService appointmentManager, IOpenAIService openAIService)
    {
        _clientSessionRepository = clientSessionRepository;
        _appointmentManager = appointmentManager;
        _openAIService = openAIService;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        var confirm = await _openAIService.SendToOpenAIAsync(PromptTemplates.ConfirmBooking(message));

        if (bool.Parse(confirm))
        {
            try
            {
                var timeRange = session.TempTimeRaw.Split('-')[0];
                var dateStr = session.TempDateRaw;

                if (!DateTime.TryParse($"{dateStr} {timeRange}", out DateTime appointmentDateTime))
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                    return "Hubo un problema con la fecha y hora seleccionadas. Por favor, intenta nuevamente.";
                }

                var requestCreate = new CreateAppointmentRequest()
                {
                    ClientId = client.Id,
                    ServiceId = session.ServiceId.Value,
                    AssignedResourceId = session.AssignedResourceId,
                    DateTime = appointmentDateTime,
                };

                var result = await _appointmentManager.CreateAsync(requestCreate);

                if (!result.Success)
                {
                    await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                    return $"Hubo un problema al crear la reserva: {result.Description}\n\nEscribe *volver* para regresar al menú principal.";
                }

                // Si es una reprogramación, eliminar el turno anterior
                if (!string.IsNullOrEmpty(session.AppointmentToReescheduleJson))
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, int>>(session.AppointmentToReescheduleJson);
                    if (data != null && data.TryGetValue("AppointmentIdToReschedule", out var appointmentId))
                    {
                        var res = await _appointmentManager.DeleteAsync(appointmentId);
                        if (!res.Success) {
                            await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                            return $"Hubo un problema al crear la reserva: {result.Description}\n\nEscribe *volver* para regresar al menú principal.";
                        }
                        session.AppointmentToReescheduleJson = null;
                        await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                        return "¡Reserva confirmada con éxito! Te esperamos.\n\nEscribe *volver* para regresar al menú principal.";
                    }
                }

                await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                return "¡Reserva confirmada con éxito! Te esperamos.\n\nEscribe *volver* para regresar al menú principal.";
            }
            catch (Exception ex)
            {
                await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
                return $"Hubo un problema al crear la reserva: {ex.Message}\n\nEscribe *volver* para regresar al menú principal.";
            }
        }
        else
        {
            await _clientSessionRepository.UpdateWithStepAsync(session, ClientStep.None);
            return $"Reserva cancelada. ¿Deseas hacer otra cosa? \n\nEscribe *volver* para regresar al menú principal.";
        }
    }
}