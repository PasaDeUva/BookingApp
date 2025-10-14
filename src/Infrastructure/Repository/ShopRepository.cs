using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Repository;

public class ShopRepository : IShopRepository
{
    private readonly AppDbContext _context;

    public ShopRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tuple<List<Shop>, int>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? id = null, string? name = null,
                                                          string? instance = null, bool? isActive = null, bool? assistanceEnable = null)
    {
        var query = _context.Shops.AsQueryable();

        if (!string.IsNullOrEmpty(name)) query = query.Where(x => x.Name.Contains(name));

        if (!string.IsNullOrEmpty(instance)) query = query.Where(x => x.Name.Contains(instance));

        if (id.HasValue) query = query.Where(x => x.Id == id);

        if (isActive.HasValue) query = query.Where(x => x.IsActive == isActive.Value);

        if (assistanceEnable.HasValue) query = query.Where(x => x.AssistanceEnable == assistanceEnable.Value);

        var total = await query.CountAsync();

        var result = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new Tuple<List<Shop>, int>(result, total);
    }
    public async Task<int> CreateAsync(Shop shop)
    {
        shop.CreatedAt = DateTime.UtcNow;
        _context.Shops.Add(shop);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> UpdateAsync(Shop shop)
    {
        var existing = await _context.Shops.FindAsync(shop.Id);
        if (existing == null) return 0;

        existing.Name = shop.Name;
        existing.Instance = shop.Instance;
        //existing.EvolutionApiKey = shop.EvolutionApiKey;
        //existing.OpenAIApiKey = shop.OpenAIApiKey;
        existing.AssistanceEnable = shop.AssistanceEnable;
        existing.IsActive = shop.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _context.SaveChangesAsync();
    }
    public async Task<int> DeleteAsync(int id)
    {
        var shop = await _context.Shops.FindAsync(id);
        if (shop == null) return 0;

        shop.IsActive = false;
        return await _context.SaveChangesAsync();
    }
    public async Task<Shop?> GetByIdAsync(int id)
    {
        return await _context.Shops.FindAsync(id);
    }
    public async Task<bool> GetAssistanceStatus()
    {
        var shop = await _context.Shops.FindAsync();
        if (shop == null) return false;

        return shop.AssistanceEnable;
    }
    public async Task<int> UpdateAssistanceEnableAsync()
    {
        var shop = await _context.Shops.FindAsync();
        if (shop == null) return 0;

        shop.AssistanceEnable = !shop.AssistanceEnable;
        shop.UpdatedAt = DateTime.UtcNow;
        return await _context.SaveChangesAsync();
    }
}