using BotWhatsapp.Application.Features.Calendar.Create;
using BotWhatsapp.Application.Features.Calendar.Delete; // New import
using BotWhatsapp.Application.Features.Calendar.Get;
using BotWhatsapp.Application.Features.Calendar.Update;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BotWhatsapp.WebApi.Controllers;
[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseModel<bool>>> CreateCalendar([FromBody] CalendarVM request)
    {
        var command = new CreateCalendarCommand { Days = request.Days, StartTime = request.StartTime, EndTime = request.EndTime, OwnerId = request.OwnerId, OwnerType = request.OwnerType };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseModel<bool>>> UpdateCalendar(int id, [FromBody] CalendarDto calendarDto)
    {
        calendarDto.Id = id;
        var command = new UpdateCalendarCommand { Calendar = calendarDto };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ResponseModel<List<CalendarDto>>>> GetAllCalendars()
    {
        var query = new GetAllCalendarsQuery();
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("day/{day}")]
    public async Task<ActionResult<ResponseModel<CalendarDto>>> GetCalendarByDay(DayOfWeek day)
    {
        var query = new GetCalendarByDayQuery(day);
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseModel<CalendarDto>>> GetCalendarById(int id)
    {
        var query = new GetCalendarByIdQuery { Id = id };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseModel<bool>>> DeleteCalendar(int id)
    {
        var command = new DeleteCalendarCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}