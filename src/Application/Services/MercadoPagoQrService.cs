using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using QRCoder;
using System.Drawing;

public class MercadoPagoQrService
{
    public MercadoPagoQrService(string accessToken)
    {
        MercadoPagoConfig.AccessToken = accessToken;
    }

    public async Task<string> GenerarQrDinamicoAsync(string descripcion, decimal monto, string reservaId)
    {
        var paymentRequest = new PaymentCreateRequest
        {
            TransactionAmount = monto,
            Description = descripcion,
            PaymentMethodId = "account_money", // puede omitirse para dejar al usuario elegir
            ExternalReference = reservaId,
            NotificationUrl = "https://tuservidor.com/api/pagos/notificacion"
        };

        var client = new PaymentClient();
        var payment = await client.CreateAsync(paymentRequest);

        // Generar QR a partir del link de pago
        var qrText = $"https://www.mercadopago.com.ar/payments/{payment.Id}";
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);

        using var qrImage = qrCode.GetGraphic(20);
        using var stream = new MemoryStream();
        qrImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        var base64 = Convert.ToBase64String(stream.ToArray());

        // Devolvemos el QR como imagen en base64 (ideal para enviarlo por WhatsApp o mostrarlo en frontend)
        return $"data:image/png;base64,{base64}";
    }
}
