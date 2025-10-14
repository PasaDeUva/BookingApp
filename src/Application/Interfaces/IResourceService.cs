using BotWhatsapp.Application.Features.Resource.Create;
using BotWhatsapp.Application.Features.Resource.GetResource;
using BotWhatsapp.Application.Features.Resource.Update;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IResourceService
{
    Task<ResponseModel<PaginationResponse<ResourceDto>>> GetAllAsync(ResourceListRequest request);
    Task<ResponseModel<PaginationResponse<ResourceDto>>> GetAllAsync();
    Task<ResponseModel<bool>> CreateAsync(CreateResourceRequest request);
    Task<ResponseModel<bool>> UpdateAsync(UpdateResourceRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<ResourceDto?>> GetByIdAsync(int id);
    Task<ResponseModel<List<ServiceDto>>> GetServicesByResourceIdAsync(int resourceId);
}