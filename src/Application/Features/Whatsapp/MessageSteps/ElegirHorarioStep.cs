using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using MediatR;
using System.Text.Json;

namespace BotWhatsapp.Application.Features.Whatsapp.MessageSteps;

public class ElegirHorarioStep : IMessageStepStrategy
{
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IServiceManager _serviceManager;

    public ElegirHorarioStep(IClientSessionRepository clientSessionRepository, IServiceManager serviceManager)
    {
        _clientSessionRepository = clientSessionRepository;
        _serviceManager = serviceManager;
    }

    public async Task<string> ExecuteAsync(ClientSession session, string message, Domain.Entities.Client client)
    {
        if (message.Trim().ToLower() == "más")
        {
            session.TimeSlotPage++;
            await _clientSessionRepository.UpdateAsync(session);

            var savedSlots = JsonSerializer.Deserialize<List<SlotInfo>>(session.AvailableTimeSlotsJson);

            return ShowTimeSlotPage(savedSlots, session.TimeSlotPage,
                (await _serviceManager.GetByIdAsync(session.ServiceId.Value)).Data.Name,
                (await _serviceManager.GetByIdAsync(session.ServiceId.Value)).Data.Price,
                session.TempDateRaw);
        }
        else if (message.Trim().ToLower() == "anterior" && session.TimeSlotPage > 0)
        {
            session.TimeSlotPage--;
            await _clientSessionRepository.UpdateAsync(session);

            var savedSlots = JsonSerializer.Deserialize<List<SlotInfo>>(session.AvailableTimeSlotsJson);

            return ShowTimeSlotPage(savedSlots, session.TimeSlotPage,
                (await _serviceManager.GetByIdAsync(session.ServiceId.Value)).Data.Name,
                (await _serviceManager.GetByIdAsync(session.ServiceId.Value)).Data.Price,
                session.TempDateRaw);
        }

        if (!int.TryParse(message, out int timeOption) || timeOption < 1)
        {
            return "Por favor, selecciona una opción válida o escribe 'más' para ver más horarios.";
        }
        var availableTimeSlots = await _serviceManager.GetAvailableTimeSlotsAsync(session.ServiceId.Value, session.TempDateRaw);
        availableTimeSlots = availableTimeSlots.Where(s =>
        {
            var startTimeString = s.TimeSlot.Split('-')[0].Trim();
            if (DateTime.TryParse(startTimeString, out DateTime startTime))
            {
                var appointmentDateTime = DateTime.Parse(session.TempDateRaw + " " + startTime.ToString("HH:mm"));
                return appointmentDateTime > DateTime.Now;
            }
            return false;
        }).ToList();

        //var availableTimeSlots = JsonSerializer.Deserialize<List<SlotInfo>>(session.AvailableTimeSlotsJson);
        int realIndex = (session.TimeSlotPage * 5) + (timeOption - 1);

        if (realIndex >= availableTimeSlots.Count)
        {
            return $"Por favor, selecciona una opción válida.";
        }

        var selectedSlot = availableTimeSlots[realIndex];
        session.TempTimeRaw = selectedSlot.TimeSlot;
        session.AssignedResourceId = selectedSlot.ResourceId;
        session.CurrentStep = ClientStep.ConfirmingAppointment;
        await _clientSessionRepository.UpdateAsync(session);

        var serviceDetails = await _serviceManager.GetByIdAsync(session.ServiceId.Value);

        return $"Resumen de tu reserva:\n" +
               $"- Servicio: {serviceDetails.Data.Name}\n" +
               $"- Fecha: {session.TempDateRaw}\n" +
               $"- Horario: {session.TempTimeRaw} {selectedSlot.ResourceName}\n" +
               $"- Precio: ${serviceDetails.Data.Price:N2}\n\n" +
               "¿Confirmás esta reserva? (Sí/No)";
    }

    private string ShowTimeSlotPage(List<SlotInfo> allTimeSlots, int page, string serviceName, decimal servicePrice, string date)
    {
        const int slotsPerPage = 15;
        var startIndex = page * slotsPerPage;
        var endIndex = Math.Min(startIndex + slotsPerPage, allTimeSlots.Count);

        if (startIndex >= allTimeSlots.Count)
        {
            return "No hay más horarios disponibles. Escribe 'anterior' para ver horarios previos.";
        }

        var pageSlots = allTimeSlots.Skip(startIndex).Take(slotsPerPage).ToList();
        var timeText = string.Join("\n", pageSlots.Select((slot, i) => $"{i + 1}. {slot.TimeSlot} {slot.ResourceName}"));

        var message = $"Has seleccionado {serviceName} por ${servicePrice:N2} para el día {date}.\n\n";
        message += "Horarios disponibles (página {page + 1}):";
        message += $"\n{timeText}\n\n";
        message += "Escribe el número del horario que prefieres.\n";

        if (endIndex < allTimeSlots.Count)
        {
            message += "Escribe 'más' para ver más horarios.\n";
        }
        if (page > 0)
        {
            message += "Escribe 'anterior' para ver horarios previos.\n";
        }

        return message;
    }
}