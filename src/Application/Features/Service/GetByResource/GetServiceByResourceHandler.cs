
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.GetByResource
{
    public class GetServiceByResourceHandler : IRequestHandler<GetServiceByResourceRequest, ResponseModel<List<ServiceDto>>>
    {
        private readonly IServiceManager _serviceManager;

        public GetServiceByResourceHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<ResponseModel<List<ServiceDto>>> Handle(GetServiceByResourceRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.GetServicesByResourceAsync(request.ResourceId);
        }
    }
}
