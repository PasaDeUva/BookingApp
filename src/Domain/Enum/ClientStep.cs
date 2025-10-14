namespace BotWhatsapp.Domain.Enums;

public enum ClientStep
{
    None,
    AskedName,
    MainMenu,
    Ready,
    AskingDate,
    ChooseDuration,
    AskingTime,
    ConfirmingAppointment,
    AskingReprogramDate,
    AskingReprogramTime,
    ConfirmingReprogram,
    ConfirmingCancel,
    RescheduleAppointment,
    WaitingForRescheduleChoice,
    ViewAppointment,
    CancelAppointment,
    WaitingForCancellationChoice
}