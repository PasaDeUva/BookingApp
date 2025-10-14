using BotWhatsapp.Application.Features.Service.Create;
using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Features.Service.Update;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IServiceManager
{
    Task<ResponseModel<PaginationResponse<ServiceDto>>> GetAllAsync(ServiceListRequest request);
    Task<ResponseModel<bool>> CreateAsync(CreateServiceRequest request);
    Task<ResponseModel<bool>> UpdateAsync(UpdateServiceRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<ServiceDto?>> GetByIdAsync(int id);
    Task<ResponseModel<List<ServiceDto>>> GetServicesByResourceAsync(int userId);
    Task<List<SlotInfo>> GetAvailableTimeSlotsAsync(int serviceId, string dateString);
    Task<List<SlotInfo>> GetAvailableTimeSlotsWithResourceAsync(int serviceId, string dateString);
}
