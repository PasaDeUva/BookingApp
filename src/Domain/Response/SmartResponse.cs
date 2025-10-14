using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Domain.Response;

public class SmartResponse
{
    public string Action { get; set; } = "";
    public string ResponseText { get; set; }
    public string SuggestedDate { get; set; }
    public string SuggestedTime { get; set; }

    public string Intent => Action;

    public bool Confirmed { get; set; }
    public string ConfirmedResponse { get; set; }

    public DateTime? ParsedDate => DateTime.TryParse(SuggestedDate, out var d) ? d : null;
    public TimeSpan? ParsedTime => TimeSpan.TryParse(SuggestedTime, out var t) ? t : null;

    //public ClientStep NextStep =>
    //    Action switch
    //    {
    //        "crear_turno" => ClientStep.AskingDate,
    //        "reprogramar_turno" => ClientStep.AskingReprogramDate,
    //        "cancelar_turno" => ClientStep.ConfirmingCancel,
    //        _ => ClientStep.Ready
    //    };
}
