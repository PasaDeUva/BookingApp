using BotWhatsapp.Application.Features.Service.Create;
using BotWhatsapp.Application.Features.Service.Delete;
using BotWhatsapp.Application.Features.Service.GetById;
using BotWhatsapp.Application.Features.Service.GetByResource;
using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Features.Service.Update;
using BotWhatsapp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IloginService _loginManager;

        public ServicesController(IMediator mediator, IloginService loginManager)
        {
            _mediator = mediator;
            _loginManager = loginManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ServiceListRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateServiceRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteServiceRequest(id);
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = new GetServiceByIdRequest(id);
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("by-resource/{id}")]
        public async Task<IActionResult> GetServicesByResource(int id)
        {
            var request = new GetServiceByResourceRequest(id);
            return Ok(await _mediator.Send(request));
        }
    }
}
