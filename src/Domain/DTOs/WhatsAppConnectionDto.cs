using System.Text.Json.Serialization;

namespace BotWhatsapp.Application.DTOs;
public class WhatsAppConnectionDto
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("pairingCode")]
    public string? PairingCode { get; set; }

    [JsonPropertyName("base64")]
    public string? Base64 { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}

public class WhatsAppDisconnectionDto
{
    public string Status { get; set; }
    public bool Error { get; set; }
    public WhatsAppDisconnectionResponse Response { get; set; }
}

public class WhatsAppDisconnectionResponse
{
    public string Message { get; set; }
}
