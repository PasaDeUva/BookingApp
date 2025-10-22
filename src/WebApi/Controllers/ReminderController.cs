using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
/// <summary>
/// ***************PROBABLEMENTE DESAPAREZCA**********************
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderManager;
    
    public ReminderController(IReminderService reminderManager)
    {
        _reminderManager = reminderManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _reminderManager.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ReminderDto>> Create([FromBody] ReminderDto reminderDto)
    {
        var created = await _reminderManager.CreateAsync(reminderDto);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _reminderManager.DeleteAsync(id);
        return NoContent();
    }
}