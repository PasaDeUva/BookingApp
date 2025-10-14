using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using System.Collections.Concurrent;

namespace BotWhatsapp.Application.Services;

public class SessionService : ISessionService
{
    private static readonly ConcurrentDictionary<string, ClientSession> _sessions = new();

    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientSessionRepository _clientSessionRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IOpenAIService _openAIService;

    public SessionService(
        IAppointmentRepository appointmentRepository,
        IClientSessionRepository clientSessionRepository,
        IServiceRepository serviceRepository,
        IClientRepository clientRepository,
        IOpenAIService openAIService)
    {
        _appointmentRepository = appointmentRepository;
        _clientSessionRepository = clientSessionRepository;
        _clientRepository = clientRepository;
        _openAIService = openAIService;
        _serviceRepository = serviceRepository;
    }

    public async Task<string> HandleNameRequestAsync(string phoneNumber, string message, ClientSession session)
    {
        if (string.IsNullOrEmpty(session.CurrentStep.ToString()))
        {
            session.CurrentStep = ClientStep.AskedName;
            await _clientSessionRepository.UpdateAsync(session);
            return "👋 ¡Hola! ¿Cómo te llamás?";
        }
        if (session.CurrentStep == ClientStep.AskedName)
        {
            var name = await _openAIService.SendToOpenAIAsync($"Extrae únicamente el nombre o apellido de la siguiente frase: {message}. Eliminando cualquier otra palabra, " +
                $"apodo, adjetivo o texto adicional. Devuelve el resultado en el formato: Nombre Apellido, con la primera letra de cada palabra en mayúscula. " +
                $"Si solo hay un nombre o solo hay un apellido, devuélve solo eso.Ejemplo: de " +
                $"'Santiago aceituna acosta' devolver 'Santiago Acosta', de 'Santiago aceituna' devolver 'Santiago' ");
            await _clientRepository.CreateAsync(new Client { PhoneNumber = phoneNumber, Name = name });
            await _clientSessionRepository.DeleteAsync(phoneNumber);
            return $"✅ Gracias {name}, ahora sí.\n" + ShowMainMenu();
        }
        return "⚠️ No entendí. ¿Podés decirme tu nombre?";
    }
    public string ShowMainMenu()
    {
        return $"👋 ¿Qué querés hacer?\n" +
        "1️⃣ *Crear turno*\n" +
        "2️⃣ *Reprogramar turno*\n" +
        "3️⃣ *Cancelar turno*\n" +
        "4️⃣ *Ver turnos*\n" +
        "✏️ Escribí el número de la opción.\n" +
        "🧭 Escribí *volver* en cualquier momento para cancelar.";
    }

    //public async Task<string> StartCreateAppointmentFlowAsync(Client client, string message, ClientSession session)
    //{
    //    session.Intent = "crear_turno";
    //    if (string.IsNullOrEmpty(session.CurrentStep.ToString()))
    //    {
    //        session.CurrentStep = ClientStep.AskingDate;
    //        await _clientSessionRepository.UpdateAsync(session);
    //        return "📅 ¿Para qué día y hora querés el turno?";
    //    }
    //    if (session.CurrentStep == ClientStep.AskingDate)
    //    {
    //        var date = await _openAIService.ExtractDateFromText(message);
    //        if (date == null)
    //            return "⚠️ No entendí la fecha. Por favor, escribila de nuevo.";

    //        session.TempDate = date;
    //        session.CurrentStep = "esperando_servicio";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        var servicios = await _serviceRepository.GetAllAsync();
    //        var opciones = string.Join("\n", servicios.Item1.Select((s, i) => $"{i + 1}. {s.Name}"));
    //        return $"✂️ ¿Qué servicio querés?\n{opciones}";
    //    }

    //    if (session.CurrentStep == "esperando_servicio")
    //    {
    //        var servicios = await _serviceRepository.GetAllAsync();
    //        if (!int.TryParse(message, out var index) || index < 1 || index > servicios.Item2)
    //            return "⚠️ Opción inválida. Elegí el número del servicio.";

    //        var servicio = servicios.Item1[index - 1];
    //        session.ServiceId = servicio.Id;

    //        var peluqueros = servicio.AssignedResourceIds.ToList();
    //        if (peluqueros != null && peluqueros.Count > 0)
    //        {
    //            session.CurrentStep = "esperando_peluquero";
    //            await _clientSessionRepository.UpdateAsync(session);

    //            var opciones = string.Join("\n", peluqueros.Select((u, i) => $"{i + 1}."));
    //            return $"💇‍♂️ ¿Con quién preferís atenderte?\n{opciones}";
    //        }
    //        else
    //        {
    //            session.AssignedResourceId = null;
    //            session.CurrentStep = "confirmar_turno";
    //            await _clientSessionRepository.UpdateAsync(session);
    //            return GetTurnoResumen(session, servicio.Name, null);
    //        }
    //    }

    //    if (session.CurrentStep == "esperando_peluquero")
    //    {
    //        var servicio = await _serviceRepository.GetByIdAsync(session.ServiceId.Value);
    //        var peluqueros = servicio.ResourceServices?.Select(es => es.Resource).ToList();

    //        if (!int.TryParse(message, out var index) || index < 1 || index > peluqueros.Count)
    //            return "⚠️ Opción inválida. Elegí el número del peluquero.";

    //        session.AssignedResourceId = peluqueros[index - 1].Id;
    //        session.CurrentStep = "confirmar_turno";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        return GetTurnoResumen(session, servicio.Name, peluqueros[index - 1].Name);
    //    }

    //    if (session.CurrentStep == "confirmar_turno")
    //    {
    //        if (await isTrue(message))
    //        {
    //            var servicio = await _serviceRepository.GetByIdAsync(session.ServiceId.Value);
    //            var fechaInicio = session.TempDate!.Value;
    //            var fechaFin = fechaInicio.AddMinutes(servicio.DurationInMinutes);

    //            if (session.AssignedResourceId.HasValue)
    //            {
    //                var overlappingAppointments = await _appointmentRepository
    //                    .GetByResourceIdBetweenDatesAsync(session.AssignedResourceId.Value, fechaInicio, fechaFin);

    //                if (overlappingAppointments.Any())
    //                {
    //                    return "⚠️ Ese peluquero ya tiene un turno en ese horario. Elegí otra fecha u otro peluquero.";
    //                }
    //            }

    //            var turno = new Appointment
    //            {
    //                ClientId = client.Id,
    //                ServiceId = session.ServiceId.Value,
    //                DateTime = fechaInicio,
    //                Status = "Confirmed",
    //                AssignedResourceId = session.AssignedResourceId
    //            };

    //            await _appointmentRepository.CreateAsync(turno);
    //            await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //            return "🎉 ¡Turno confirmado! Te esperamos.";
    //        }

    //        await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //        return "❌ Turno cancelado.";
    //    }

    //    return "❓ No entendí. ¿Querés confirmar el turno?";
    //}

    //public async Task<string> StartRescheduleAppointmentFlowAsync(Client client, string message, ClientSession session)
    //{
    //    session.Intent = "reprogramar_turno";

    //    if (string.IsNullOrEmpty(session.CurrentStep))
    //    {
    //        var appointments = await _appointmentRepository.GetUpcomingByClientIdAsync(client.Id);
    //        if (!appointments.Any())
    //        {
    //            await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //            return "🔔 No tenés turnos activos para reprogramar.";
    //        }

    //        var appt = appointments.OrderBy(a => a.DateTime).First();
    //        session.AppointmentId = appt.Id;
    //        session.CurrentStep = "esperando_nueva_fecha";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        return $"📅 Estás reprogramando tu turno del {appt.DateTime:dddd dd/MM HH:mm}.\n¿Para qué nueva fecha y hora lo querés?";
    //    }

    //    if (session.CurrentStep == "esperando_nueva_fecha")
    //    {
    //        var nuevaFecha = await _openAIService.ExtractDateFromText(message);
    //        if (nuevaFecha == null)
    //            return "⚠️ No entendí la fecha. Escribila de nuevo.";

    //        session.TempDate = nuevaFecha;
    //        session.CurrentStep = "esperando_servicio";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        var servicios = await _serviceRepository.GetAllAsync();
    //        var opciones = string.Join("\n", servicios.Item1.Select((s, i) => $"{i + 1}. {s.Name}"));
    //        return $"✂️ ¿Qué servicio querés para este nuevo turno?\n{opciones}";
    //    }

    //    if (session.CurrentStep == "esperando_servicio")
    //    {
    //        var servicios = await _serviceRepository.GetAllAsync();
    //        if (!int.TryParse(message, out var index) || index < 1 || index > servicios.Item2)
    //            return "⚠️ Opción inválida. Elegí el número del servicio.";

    //        var servicio = servicios.Item1[index - 1];
    //        session.ServiceId = servicio.Id;

    //        var peluqueros = servicio.AssignedResourceIds.ToList();
    //        if (peluqueros != null && peluqueros.Count > 0)
    //        {
    //            session.CurrentStep = "esperando_peluquero";
    //            await _clientSessionRepository.UpdateAsync(session);

    //            var opciones = string.Join("\n", peluqueros.Select((u, i) => $"{i + 1}."));
    //            return $"💇‍♂️ ¿Con quién preferís atenderte?\n{opciones}";
    //        }
    //        else
    //        {
    //            session.AssignedResourceId = null;
    //            session.CurrentStep = "confirmar_reprogramacion";
    //            await _clientSessionRepository.UpdateAsync(session);

    //            return GetTurnoResumen(session, servicio.Name, null, true);
    //        }
    //    }

    //    if (session.CurrentStep == "esperando_peluquero")
    //    {
    //        var servicio = await _serviceRepository.GetByIdAsync(session.ServiceId.Value);
    //        var peluqueros = servicio.ResourceServices?.Select(es => es.Resource).ToList();

    //        if (!int.TryParse(message, out var index) || index < 1 || index > peluqueros.Count)
    //            return "⚠️ Opción inválida. Elegí el número del peluquero.";

    //        session.AssignedResourceId = peluqueros[index - 1].Id;
    //        session.CurrentStep = "confirmar_reprogramacion";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        return GetTurnoResumen(session, servicio.Name, peluqueros[index - 1].Name, true);
    //    }

    //    if (session.CurrentStep == "confirmar_reprogramacion")
    //    {
    //        if (await isTrue(message))
    //        {
    //            var servicio = await _serviceRepository.GetByIdAsync(session.ServiceId.Value);
    //            var fechaInicio = session.TempDate!.Value;
    //            var fechaFin = fechaInicio.AddMinutes(servicio.DurationInMinutes);

    //            if (session.AssignedResourceId.HasValue)
    //            {
    //                var overlapping = await _appointmentRepository.GetByResourceIdBetweenDatesAsync(session.AssignedResourceId.Value, fechaInicio, fechaFin);

    //                if (overlapping.Any())
    //                    return "⚠️ Ese peluquero ya tiene otro turno en ese horario. Elegí otra fecha.";

    //            }

    //            await _appointmentRepository.UpdateAsync(new Appointment
    //            {
    //                Id = session.AppointmentId.Value,
    //                DateTime = fechaInicio,
    //                ServiceId = session.ServiceId.Value,
    //                AssignedResourceId = session.AssignedResourceId
    //            });

    //            await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //            return "✅ ¡Turno reprogramado con éxito!";
    //        }

    //        await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //        return "❌ Reprogramación cancelada.";
    //    }

    //    return "❓ No entendí. Podés escribir *volver* para cancelar.";
    //}

    //public async Task<string> StartCancelAppointmentFlowAsync(Client client, string message, ClientSession session)
    //{
    //    if (string.IsNullOrEmpty(session.CurrentStep))
    //    {
    //        var appt = await _appointmentRepository.GetNextAppointmentByClientIdAsync(client.Id);
    //        if (appt == null)
    //        {
    //            await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //            return "🔔 No tenés turnos activos para cancelar.";
    //        }

    //        session.AppointmentId = appt.Id;
    //        session.CurrentStep = "confirmar_cancelacion";
    //        await _clientSessionRepository.UpdateAsync(session);

    //        return $"❓ ¿Estás seguro que querés cancelar tu turno del *{appt.DateTime:dddd dd/MM HH:mm}*?\n✏️ Respondé *sí* para confirmar o *volver* para cancelar.";
    //    }

    //    if (session.CurrentStep == "confirmar_cancelacion")
    //    {
    //        if (await isTrue(message))
    //        {
    //            await _appointmentRepository.CancelAsync(session.AppointmentId.Value);
    //            await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //            return "✅ Tu turno fue cancelado.";
    //        }

    //        await _clientSessionRepository.DeleteAsync(client.PhoneNumber);
    //        return "❌ Cancelación anulada.";
    //    }

    //    return "❓ No entendí. Podés escribir *volver* para salir.";
    //}

    //public async Task<string> StartListAppointmentsFlowAsync(Client client, ClientSession session)
    //{
    //    var appointments = await _appointmentRepository.GetUpcomingByClientIdAsync(client.Id);
    //    await _clientSessionRepository.DeleteAsync(client.PhoneNumber);

    //    if (!appointments.Any())
    //        return "📭 No tenés turnos programados.";

    //    var list = string.Join("\n", appointments.OrderBy(a => a.DateTime).Select(a => $"• 🗓️ {a.DateTime:dddd dd/MM HH:mm}"));
    //    return $"📅 Estos son tus próximos turnos:\n{list}";
    //}

    //private async Task<bool> isTrue(string message)
    //{
    //    var res = await _openAIService.SendToOpenAIAsync($"Necesito que me devuelvas como string un booleano. true si es afirmativa, false si no. Mensaje: {message.Trim().ToLower()}");
    //    return bool.Parse(res);
    //}

    //private string GetTurnoResumen(ClientSession session, string servicioNombre, string? peluqueroNombre, bool reprogramacion = false)
    //{
    //    return (reprogramacion ? "🔁 Vas a reprogramar tu turno:\n" : "📝 Vas a agendar:\n") +
    //           $"📅 Fecha: *{session.TempDate:dddd dd/MM HH:mm}*\n" +
    //           $"✂️ Servicio: *{servicioNombre}*\n" +
    //           $"💈 Peluquero: *{(peluqueroNombre ?? "Cualquiera disponible")}*\n" +
    //           $"¿Confirmás?\n✏️ Respondé *sí* para confirmar o *volver* para cancelar.";
    //}
}
