using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/pagos")]
public class PaymentController : ControllerBase
{
    private readonly MercadoPagoQrService _qrService;

    public PaymentController(MercadoPagoQrService qrService)
    {
        _qrService = qrService;
    }

    [HttpGet("qr")]
    public async Task<IActionResult> GenerarQr([FromQuery] string reservaId, [FromQuery] decimal monto)
    {
        var qrBase64 = await _qrService.GenerarQrDinamicoAsync("Reserva de turno", monto, reservaId);
        return Ok(new { qr = qrBase64 });
    }
}
