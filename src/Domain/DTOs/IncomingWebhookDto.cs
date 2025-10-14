namespace BotWhatsapp.Domain.Dtos;

public class IncomingWebhookDto
{
    public string Event { get; set; }
    public string Instance { get; set; }
    public WebhookData Data { get; set; }
    public string Sender { get; set; }
}

public class WebhookData
{
    public WebhookKey Key { get; set; }
    public WebhookMessage Message { get; set; }
}

public class WebhookKey
{
    public string RemoteJid { get; set; }
}

public class WebhookMessage
{
    public ExtendedTextMessage ExtendedTextMessage { get; set; }
    public string Conversation { get; set; }
}

public class ExtendedTextMessage
{
    public string Text { get; set; }
}
