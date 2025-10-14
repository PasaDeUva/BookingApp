using BotWhatsapp.Application.Features.User.DeleteUser;
using BotWhatsapp.Application.Features.User.Register;
using BotWhatsapp.Application.Features.User.UpdateUser;
using BotWhatsapp.Application.Features.User.UserList;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IloginService _loginManager;
    private readonly IMediator _mediator;

    public UserController(IloginService loginManager, IMediator mediator)
    {
        _loginManager = loginManager;
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserListRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut("users")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(DeleteUserRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

}