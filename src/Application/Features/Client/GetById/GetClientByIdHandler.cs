using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetById
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdRequest, ResponseModel<ClientDto?>>
    {
        private readonly IClientService _clientManager;
        
        public GetClientByIdHandler(IClientService clientManager)
        {
            _clientManager = clientManager;
        }
        
        public async Task<ResponseModel<ClientDto?>> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
        {
            return await _clientManager.GetByIdAsync(request.Id);
        }
    }
}