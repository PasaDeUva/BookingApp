using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Delete
{
    public class DeleteClientHandler : IRequestHandler<DeleteClientRequest, ResponseModel<bool>>
    {
        private readonly IClientService _clientManager;
        
        public DeleteClientHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<bool>> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
        {
            return await _clientManager.DeleteAsync(request.Id);
        }
    }
}