using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Update;

public class UpdateResourceHandler : IRequestHandler<UpdateResourceRequest, ResponseModel<bool>>
{
    private readonly IResourceService _resourceManager;
    public UpdateResourceHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }
    public async Task<ResponseModel<bool>> Handle(UpdateResourceRequest request, CancellationToken cancellationToken)
    {
        return await _resourceManager.UpdateAsync(request);
    }
}
