using BotWhatsapp.Application.Features.Whatsapp.DisconnectWhatsapp;
using BotWhatsapp.Application.Features.Whatsapp.GetQR;
using BotWhatsapp.Application.Features.Whatsapp.Incoming;
using BotWhatsapp.Application.Features.Whatsapp.MercadoPagoWebhook;
using BotWhatsapp.Application.Features.Whatsapp.RestartInstance;
using BotWhatsapp.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsappController : ControllerBase
{
    private readonly IMediator _mediator;

    public WhatsappController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("incoming")]
    public async Task<IActionResult> Incoming()
    {
        try
        {
            using var reader = new StreamReader(Request.Body);
            var bodyRaw = await reader.ReadToEndAsync();

            Console.WriteLine(bodyRaw);

            var dto = JsonSerializer.Deserialize<IncomingWebhookDto>(bodyRaw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var response = await _mediator.Send(new IncomingCommand(dto));
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error procesando webhook:");
            Console.WriteLine(ex.Message);
            return StatusCode(500, "❌ Error interno al procesar el webhook.");
        }
    }

    [HttpPost("mercadopago-webhook")]
    public async Task<IActionResult> MercadoPagoWebhook([FromBody] JsonElement body)
    {
        try
        {
            var response = await _mediator.Send(new MercadoPagoWebhookCommand(body));
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error en MercadoPagoWebhook:");
            Console.WriteLine(ex.Message);
            return StatusCode(500, "❌ Error procesando el webhook de Mercado Pago.");
        }
    }

    [HttpGet("qr")]
    public async Task<IActionResult> GetQR()
    {
        try
        {
            var result = await _mediator.Send(new GetQRQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener el código QR");
        }
    }

    [HttpPost("disconnect")]
    public async Task<IActionResult> DisconnectWhatsapp()
    {
        try
        {
            var result = await _mediator.Send(new DisconnectWhatsappCommand());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al desvincular WhatsApp: {ex.Message}");
        }
    }

    [HttpPut("restart")]
    public async Task<IActionResult> RestartInstance()
    {
        try
        {
            var result = await _mediator.Send(new RestartInstanceCommand());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al reiniciar instancia: {ex.Message}");
        }
    }
}

