using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;

namespace BotWhatsapp.Domain.Interfaces;

public interface IResourceRepository
{
    Task<Tuple<List<Resource>, int>> GetAllAsync(bool? isActive, int pageSize = 10,
                                                 int pageNumber = 1, int? id = null, string? name = "");
    Task<Tuple<List<Resource>, int>> GetAllAsync();
    Task<Resource?> GetByIdAsync(int id);
    Task<List<Service>> GetServicesByResourceIdAsync(int employeeId);
    Task<Resource> CreateResourceAsync(Resource resource);
    Task AddServicesToResourceAsync(int resourceId, List<int> serviceIds);
    Task<Resource> UpdateWithServicesAsync(Resource employee, List<int> serviceIds);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
}