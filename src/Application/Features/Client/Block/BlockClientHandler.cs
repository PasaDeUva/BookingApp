using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Block
{
    public class BlockClientHandler : IRequestHandler<BlockClientRequest, ResponseModel<bool>>
    {
        private readonly IClientService _clientManager;
        
        public BlockClientHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<bool>> Handle(BlockClientRequest request, CancellationToken cancellationToken)
        {
            var clientBlockRequest = new ClientBlockRequest
            {
                isBloqued = request.IsBloqued
            };
            
            return await _clientManager.BlockAsync(request.Id, clientBlockRequest);
        }
    }
}