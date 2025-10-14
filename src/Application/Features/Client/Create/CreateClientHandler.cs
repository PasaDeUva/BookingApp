using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Create
{
    public class CreateClientHandler : IRequestHandler<CreateClientRequest, ResponseModel<bool>>
    {
        private readonly IClientService _clientManager;
        
        public CreateClientHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<bool>> Handle(CreateClientRequest request, CancellationToken cancellationToken)
        {
            var clientCreateRequest = new ClientCreateRequest
            {
                PhoneNumber = request.PhoneNumber,
                Name = request.Name
            };
            
            return await _clientManager.CreateAsync(clientCreateRequest);
        }
    }
}