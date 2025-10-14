using MediatR;
using System.Text.Json;

namespace BotWhatsapp.Application.Features.Whatsapp.MercadoPagoWebhook;

public class MercadoPagoWebhookCommand : IRequest<string>
{
    public JsonElement Body { get; set; }

    public MercadoPagoWebhookCommand(JsonElement body)
    {
        Body = body;
    }
}
