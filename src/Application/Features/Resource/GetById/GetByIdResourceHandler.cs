using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.GetById;

public class GetByIdResourceHandler : IRequestHandler<GetByIdResourceRequest, ResponseModel<ResourceDto>>
{
    private readonly IResourceService _resourceManager;

    public GetByIdResourceHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public async Task<ResponseModel<ResourceDto?>> Handle(GetByIdResourceRequest request, CancellationToken cancellationToken)
    {
        return await _resourceManager.GetByIdAsync(request.Id);

    }

}
