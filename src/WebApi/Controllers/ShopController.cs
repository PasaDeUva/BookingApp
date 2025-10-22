using BotWhatsapp.Application.Features.Shop.Create;
using BotWhatsapp.Application.Features.Shop.Delete;
using BotWhatsapp.Application.Features.Shop.GetById;
using BotWhatsapp.Application.Features.Shop.GetList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

//[Authorize]
[AllowAnonymous] // Uncomment this line to allow anonymous access for testing purposes
[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly IMediator _mediator;
    public ShopController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetShopsRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = new GetByIdRequest(id);
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShopRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateShopRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var request = new DeleteShopRequest(id);
        return Ok(await _mediator.Send(request));
    }
}