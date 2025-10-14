using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetResource;

public  class ResourceListHandler : IRequestHandler<ResourceListRequest, ResponseModel<PaginationResponse<ResourceDto>>>
{
    private readonly IResourceService _resourceManager;
    public ResourceListHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }
    public async Task<ResponseModel<PaginationResponse<ResourceDto>>> Handle(ResourceListRequest request, CancellationToken cancellationToken)
    {
        return await _resourceManager.GetAllAsync(request);
    }
}
