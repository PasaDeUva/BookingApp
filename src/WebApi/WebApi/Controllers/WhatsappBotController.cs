using BotWhatsapp.Application.Features.WhatsappBot.AssistanceStatus;
using BotWhatsapp.Application.Features.WhatsappBot.GetReminders;
using BotWhatsapp.Application.Features.WhatsappBot.ScheduleReminder;
using BotWhatsapp.Application.Features.WhatsappBot.SendMassMessage;
using BotWhatsapp.Application.Features.WhatsappBot.ToggleBot;
using BotWhatsapp.Domain.EntitiesVM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class WhatsappBotController : ControllerBase
{
    private readonly IMediator _mediator;

    public WhatsappBotController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("mass-message")]
    public async Task<IActionResult> SendMassMessage([FromBody] MassMessageRequest request)
    {
        var response = await _mediator.Send(new SendMassMessageCommand(request));
        return Ok(response);
    }

    [HttpPost("schedule-reminder")]
    public async Task<IActionResult> ScheduleReminder([FromBody] ReminderRequest request)
    {
        var response = await _mediator.Send(new ScheduleReminderCommand(request));
        return Ok(response);
    }

    [HttpGet("reminders")]
    public async Task<IActionResult> GetReminders()
    {
        var response = await _mediator.Send(new GetRemindersQuery());
        return Ok(response);
    }

    [HttpGet("assistant-status")]
    public async Task<IActionResult> AssistanceStatus()
    {
        var response = await _mediator.Send(new AssistanceStatusQuery());
        return Ok(response);
    }

    [HttpPut("toggle-bot")]
    public async Task<IActionResult> ToggleBot()
    {
        var response = await _mediator.Send(new ToggleBotCommand());
        return Ok(response);
    }
}
