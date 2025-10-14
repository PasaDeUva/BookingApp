using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetByPhone
{
    public class GetClientByPhoneHandler : IRequestHandler<GetClientByPhoneRequest, ResponseModel<ClientDto?>>
    {
        private readonly IClientService _clientManager;
        
        public GetClientByPhoneHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<ClientDto?>> Handle(GetClientByPhoneRequest request, CancellationToken cancellationToken)
        {
            return await _clientManager.GetByPhoneNumberAsync(request.Phone);
        }
    }
}