using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetServiceById;

public class GetResourceByServiceIdHandler : IRequestHandler<GetResourceByServiceIdRequest, ResponseModel<List<ServiceDto>>>
{
    private readonly IResourceService _resourceManager;
    public GetResourceByServiceIdHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }
    public async Task<ResponseModel<List<ServiceDto>>> Handle(GetResourceByServiceIdRequest request, CancellationToken cancellationToken)
    {
        return await _resourceManager.GetServicesByResourceIdAsync(request.ServiceId);
    }
}
