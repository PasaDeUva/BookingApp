using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Update
{
    public class UpdateServiceHandler : IRequestHandler<UpdateServiceRequest, ResponseModel<bool>>
    {
        private readonly IServiceManager _serviceManager;
        public UpdateServiceHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<ResponseModel<bool>> Handle(UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.UpdateAsync(request);
        }
    }
}
