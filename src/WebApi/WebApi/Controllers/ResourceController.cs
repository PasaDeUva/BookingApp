using BotWhatsapp.Application.Features.Resource.Availability;
using BotWhatsapp.Application.Features.Resource.Create;
using BotWhatsapp.Application.Features.Resource.Delete;
using BotWhatsapp.Application.Features.Resource.GetById;
using BotWhatsapp.Application.Features.Resource.GetResource;
using BotWhatsapp.Application.Features.Resource.GetServiceById;
using BotWhatsapp.Application.Features.Resource.Update;
using BotWhatsapp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ResourceListRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateResourceRequest request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateResourceRequest request)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var request = new DeleteResourceRequest(id);
        return Ok(await _mediator.Send(request));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = new GetByIdResourceRequest(id);
        return Ok(await _mediator.Send(request));
    }
    [HttpGet("{id}/services")]
    public async Task<IActionResult> GetServicesByResource(int id)
    {
        var request = new GetResourceByServiceIdRequest(id);
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailability(int resourceId, DateTime date)
    {
        var request = new GetAvailabilityRequest(resourceId, date);
        return Ok(await _mediator.Send(request));
    }

}