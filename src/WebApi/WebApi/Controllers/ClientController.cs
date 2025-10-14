using BotWhatsapp.Application.Features.Client.Block;
using BotWhatsapp.Application.Features.Client.Create;
using BotWhatsapp.Application.Features.Client.Delete;
using BotWhatsapp.Application.Features.Client.GetAll;
using BotWhatsapp.Application.Features.Client.GetById;
using BotWhatsapp.Application.Features.Client.GetByPhone;
using BotWhatsapp.Application.Features.Client.Update;
using BotWhatsapp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IShopService _shopManager;
    private readonly IMediator _mediator;

    public ClientController(IShopService shopManager, IMediator mediator)
    {
        _shopManager = shopManager;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllClientRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = new GetClientByIdRequest(id);
        return Ok(await _mediator.Send(request));
    }

    [HttpGet("phone/{phone}")]
    public async Task<IActionResult> GetByPhoneNumber(string phone)
    {
        var request = new GetClientByPhoneRequest(phone);
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest clientRequest)
    {
        return Ok(await _mediator.Send(clientRequest));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientRequest clientRequest)
    {
        clientRequest.Id = id;
        return Ok(await _mediator.Send(clientRequest));
    }

    [HttpPut("block/{id}")]
    public async Task<IActionResult> BlockUser(int id, [FromBody] BlockClientRequest clientRequest)
    {
        clientRequest.Id = id;
        return Ok(await _mediator.Send(clientRequest));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var request = new DeleteClientRequest(id);
        return Ok(await _mediator.Send(request));
    }
}