using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Application.Interfaces;

public interface ISessionService
{
    Task<string> HandleNameRequestAsync(string phoneNumber, string message, ClientSession session);
    //Task<string> StartCreateAppointmentFlowAsync(Client client, string message, ClientSession session);
    string ShowMainMenu();
    //Task<string> StartRescheduleAppointmentFlowAsync(Client client, string message, ClientSession session);
    //Task<string> StartCancelAppointmentFlowAsync(Client client, string message, ClientSession session);
    //Task<string> StartListAppointmentsFlowAsync(Client client, ClientSession session);
}
