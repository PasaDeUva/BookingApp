using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;
using System.Text;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class WaitingForCancellationChoiceStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IAppointmentService _appointmentManager;

    public WaitingForCancellationChoiceStep(IClientSessionRepository clientSessionRepository, IAppointmentService appointmentManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _appointmentManager = appointmentManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        return await ProcessCancellationChoice(session, message, client);
    }

    private async Task<string> ProcessCancellationChoice(ClientSession session, string message, Domain.Entities.Client client)
    {
        var sb = new StringBuilder();
        if (!int.TryParse(message, out int choice) || string.IsNullOrWhiteSpace(session.CurrentStep.ToString()))
        {
            return "Por favor, envía un número válido.";
        }

        var response = await _appointmentManager.GetByClientIdAsync(client.Id);

        if (!response.Success || response.Data == null)
        {
            return "Hubo un error al encontrar tu turno. Por favor, intenta de nuevo más tarde o 🧭 Escribí *volver* para regresar al menú principal.";
        }

        var activeAppointments = response.Data?
            .Where(a => a.Status != Domain.Enum.AppointmentStatus.Cancelled && a.DateTime >= DateTime.Now)
            .OrderBy(a => a.DateTime)
            .ToList();

        var cancellationResponse = new ResponseModel<bool>
        {
            Success = false,
            Data = false,
            Description = "No se realizó ninguna cancelación."
        };


        if (int.TryParse(message, out int index) &&
            index >= 0 && index <= activeAppointments?.Count)
        {
            cancellationResponse = await _appointmentManager.DeleteAsync(
                activeAppointments[index - 1].Id
            );
        }

        session.CurrentStep = ClientStep.None;
        
        await _clientSessionRepository.UpdateAsync(session);

        if (cancellationResponse.Success)
        {
            sb.AppendLine($"Tu turno ha sido cancelado exitosamente.");
        }
        else
        {
            sb.AppendLine("Hubo un error al cancelar tu turno. Por favor, contacta a soporte.");
        }

        sb.AppendLine($"🧭 Escribí *volver* para regresar al menú principal.");
        return sb.ToString();
    }
}
