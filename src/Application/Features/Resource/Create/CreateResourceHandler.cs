using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Create;

public class CreateResourceHandler : IRequestHandler<CreateResourceRequest, ResponseModel<bool>>
{
    private readonly IResourceService _resourceManager;

    public CreateResourceHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }
    public Task<ResponseModel<bool>> Handle(CreateResourceRequest request, CancellationToken cancellationToken)
    {
        return _resourceManager.CreateAsync(request);
    }
}
