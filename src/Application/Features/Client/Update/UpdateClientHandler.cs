using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Update
{
    public class UpdateClientHandler : IRequestHandler<UpdateClientRequest, ResponseModel<bool>>
    {
        private readonly IClientService _clientManager;
        
        public UpdateClientHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<bool>> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var clientUpdateRequest = new ClientUpdateRequest
            {
                PhoneNumber = request.PhoneNumber,
                Name = request.Name
            };
            
            return await _clientManager.UpdateAsync(request.Id, clientUpdateRequest);
        }
    }
}