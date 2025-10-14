using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;

namespace BotWhatsapp.Domain.Interfaces;

public interface IShopRepository
{
    Task<Tuple<List<Shop>, int>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? id = null, string? name = null,
                                             string? instance = null, bool? isActive = null, bool? assistanceEnable = null);
    Task<int> CreateAsync(Shop shop);
    Task<int> UpdateAsync(Shop shop);
    Task<int> DeleteAsync(int id);
    Task<Shop?> GetByIdAsync(int id);
    Task<int> UpdateAssistanceEnableAsync();
    Task<bool> GetAssistanceStatus();
}