using BotWhatsapp.Application.Features.Client.GetAll;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetById;

public class GetAllClientHandler : IRequestHandler<GetAllClientRequest, ResponseModel<PaginationResponse<ClientDto>>>
{
    private readonly IClientService _clientManager;

    public GetAllClientHandler(IClientService clientManager)
    {
        _clientManager = clientManager;
    }

    public async Task<ResponseModel<PaginationResponse<ClientDto>>> Handle(GetAllClientRequest request, CancellationToken cancellationToken)
    {
        return await _clientManager.GetAllAsync(request);
    }
}