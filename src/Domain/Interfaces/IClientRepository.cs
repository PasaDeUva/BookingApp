using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Domain.Interfaces;

public interface IClientRepository
{
    Task<Tuple<List<Client>, int>> GetAllAsync(int? id, string phoneNumber, string name, int pageNumber, int pageSize, bool? isActive, bool? isBloqued);
    Task<int> CreateAsync(Client client);
    Task<int> UpdateAsync(Client client);
    Task<int> DeleteAsync(int id);
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByPhoneNumberAsync(string phone);
    Task<List<Client>> GetUnblockedPhoneNumbers();
}
