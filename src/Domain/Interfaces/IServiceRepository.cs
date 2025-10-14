using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;

namespace BotWhatsapp.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Tuple<List<ServiceDto>, int>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? id = null, string? name = "");
    Task<int> CreateAsync(Service service);
    Task<int> UpdateAsync(Service service, List<int> userIds);
    Task<int> DeleteAsync(int id);
    Task<Service?> GetByIdAsync(int id);
    Task<List<Service>> GetServicesByResourceAsync(int userId);
}
