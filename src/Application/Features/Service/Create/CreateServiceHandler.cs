using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Create
{
    public class CreateServiceHandler : IRequestHandler<CreateServiceRequest, ResponseModel<bool>>
    {
        private readonly IServiceManager _serviceManager;
        public CreateServiceHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<ResponseModel<bool>> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.CreateAsync(request);
        }
    }
}
