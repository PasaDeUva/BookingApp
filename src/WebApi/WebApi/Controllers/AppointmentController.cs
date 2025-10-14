using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.EntitiesVM.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _manager;
    private readonly IShopService _shopManager;

    public AppointmentController(IAppointmentService manager, IShopService shopManager)
    {
        _manager = manager;
        _shopManager = shopManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AppointmentFilters request)
    {
        return Ok(await _manager
            .GetAllAsync(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        return Ok(await _manager
            .CreateAsync(request));
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAppointmentRequest request)
    {
        return Ok(await _manager.
            UpdateAsync(request));
    }
    [HttpPut("byDate")]
    public async Task<IActionResult> UpdateByDate([FromBody] UpdateAppointmentByDateRequest request)
    {
        return Ok(await _manager.
            UpdateByDateAsync(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _manager.DeleteAsync(id));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _manager.
            GetByIdAsync(id));
    }
    [HttpGet("available/{resourceId}")]
    public async Task<IActionResult> CheckAvailability(int resourceId, [FromQuery] DateTime dateTime,
                                                                       [FromQuery] int duration = 30)
    {
        return Ok(await _manager.
            IsResourceAvailable(resourceId, dateTime, duration));
    }
}
