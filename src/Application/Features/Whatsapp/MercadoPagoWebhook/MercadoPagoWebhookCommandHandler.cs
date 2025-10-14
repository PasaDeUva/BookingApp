using BotWhatsapp.Application.Interfaces;
using MediatR;

namespace BotWhatsapp.Application.Features.Whatsapp.MercadoPagoWebhook;

public class MercadoPagoWebhookCommandHandler : IRequestHandler<MercadoPagoWebhookCommand, string>
{
    private readonly IEvolutionService _evolutionService;

    public MercadoPagoWebhookCommandHandler(IEvolutionService evolutionService)
    {
        _evolutionService = evolutionService;
    }

    public async Task<string> Handle(MercadoPagoWebhookCommand request, CancellationToken cancellationToken)
    {
        var action = request.Body.GetProperty("action").GetString();
        if (action == "payment.created" || action == "payment.updated")
        {
            long paymentId = request.Body.GetProperty("data").GetProperty("id").GetInt64();

            // Consultar el estado real del pago
            var (status, reference) = await _evolutionService.ObtenerEstadoPagoAsync(paymentId);

            if (!string.IsNullOrEmpty(reference))
            {
                // reference es el número de WhatsApp (ej: "5491122334455")
                string mensaje = status.ToLower() switch
                {
                    "approved" => "✅ ¡Gracias! Tu pago fue aprobado con éxito.",
                    "pending" => "⏳ Tu pago está pendiente. Te avisaremos cuando se confirme.",
                    "rejected" => "❌ El pago fue rechazado. Por favor intentá nuevamente.",
                    _ => $"ℹ️ Estado del pago: {status}"
                };

                await _evolutionService.SendMessageAsync(reference, mensaje);
                return "✅ Webhook procesado correctamente.";
            }
        }

        return "ℹ️ Webhook recibido, pero sin acción válida.";
    }
}
