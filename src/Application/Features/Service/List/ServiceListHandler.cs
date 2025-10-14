using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.List
{
    public class ServiceListHandler : IRequestHandler<ServiceListRequest, ResponseModel<PaginationResponse<ServiceDto>>>
    {
        private readonly IServiceManager _serviceManager;
        public ServiceListHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<ResponseModel<PaginationResponse<ServiceDto>>> Handle(ServiceListRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.GetAllAsync(request);
        }
    }
}
