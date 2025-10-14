
using BotWhatsapp.Domain.Entities;
using System.Threading.Tasks;

namespace BotWhatsapp.Domain.Interfaces;

public interface IUserRepository
{
    Task<Tuple<List<User>, int>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        int? id = null,
        string? username = null,
        string? email = null,
        string? role = null,
        bool? isActive = null);
    Task<int> CreateAsync(User user);
    Task<int> UpdateAsync(User user);
    Task<int> DeleteAsync(int id);
    Task<User> GetByRefreshTokenAsync(string refreshToken);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByOnlyIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<int> GetShopFromInstanceAsync(string instance);
}
