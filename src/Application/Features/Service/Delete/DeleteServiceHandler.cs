using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Service.Delete
{
    public class DeleteServiceHandler : IRequestHandler<DeleteServiceRequest, ResponseModel<bool>>
    {
        private readonly IServiceManager _serviceManager;
        public DeleteServiceHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<ResponseModel<bool>> Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
        {
            return await _serviceManager.DeleteAsync(request.Id);
        }
    }
}
