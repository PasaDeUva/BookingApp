using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.GetById
{
    public class GetServiceByIdHandler : IRequestHandler<GetServiceByIdRequest, ResponseModel<ServiceDto>>
    {
        private readonly IServiceManager _serviceManager;
        public GetServiceByIdHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<ResponseModel<ServiceDto>> Handle(GetServiceByIdRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.GetByIdAsync(request.Id);
        }
    }
}
