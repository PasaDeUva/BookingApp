using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Resource.Delete;

public class DeleteResourceHandler : IRequestHandler<DeleteResourceRequest, ResponseModel<bool>>
{
    private readonly IResourceService _resourceManager;
    public DeleteResourceHandler(IResourceService resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public async Task<ResponseModel<bool>> Handle(DeleteResourceRequest request, CancellationToken cancellationToken)
    {
        return await _resourceManager.DeleteAsync(request.Id);
    }
}
