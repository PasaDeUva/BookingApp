using BotWhatsapp.Application.Features.Whatsapp.MessageSteps;
using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Application.Features.Whatsapp;

public class MessageStepResolver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _strategyMap = new Dictionary<string, Type>
    {
        { "MainMenu", typeof(MenuPrincipalStep) },
        { "AskingDate", typeof(ElegirDiaStep) },
        { "ChooseDuration", typeof(ElegirDuracionStep) },
        { "AskingTime", typeof(ElegirHorarioStep) },
        { "ConfirmingAppointment", typeof(ConfirmarTurnoStep) },
        { "RescheduleAppointment", typeof(ReprogramarTurnoStep) },
        { "CancelAppointment", typeof(CancelarTurnoStep) },
        { "AskedName", typeof(EsperandoNombreStep) },
        { "WaitingForCancellationChoice", typeof(WaitingForCancellationChoiceStep) },
        { "ViewAppointment", typeof(VerTurnosStep) },
        { "WaitingForRescheduleChoice", typeof(WaitingForRescheduleChoiceStep) }
    };

    public MessageStepResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IMessageStepStrategy Resolve(ClientStep step)
    {
        if (_strategyMap.TryGetValue(step.ToString(), out var strategyType))
        {
            return (IMessageStepStrategy)_serviceProvider.GetService(strategyType);
        }
        return null; // Or a default strategy
    }
}
