using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tuple<List<User>, int>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        int? id = null,
        string? username = null,
        string? email = null,
        string? role = null,
        bool? isActive = null)
    {
        var query = _context.Users.AsQueryable();

        if (id.HasValue)
            query = query.Where(u => u.Id == id.Value);

        if (!string.IsNullOrEmpty(username))
            query = query.Where(u => u.Username == username);

        if (!string.IsNullOrEmpty(email))
            query = query.Where(u => u.Email == email);

        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role == role);

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        var total = await query.CountAsync();
        var result = await query.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return new Tuple<List<User>, int>(result, total);
    }

    public async Task<int> CreateAsync(User user)
    {
        _context.Users.Add(user);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return 0;

        user.IsActive = false;
        return await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByOnlyIdAsync(int id) =>
        await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Where(u => u.IsActive && u.Username == username)
            .FirstOrDefaultAsync();
    }
    
    public async Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<int> GetShopFromInstanceAsync(string instance)
    {
        var shop = await _context.Shops.Where(u => u.IsActive && u.Instance == instance).FirstOrDefaultAsync();
        return shop.Id;
    }
}
