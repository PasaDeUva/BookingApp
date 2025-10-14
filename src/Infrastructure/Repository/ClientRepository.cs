using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Repository;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;
    public ClientRepository(AppDbContext context) => _context = context;

    // ABM BASICO
    public async Task<Tuple<List<Client>, int>> GetAllAsync(int? id, string phoneNumber, string name, int pageNumber, int pageSize, bool? isActive, bool? isBloqued)
    {
        var query = _context.Clients.AsQueryable();
        query = query.Where(c => c.IsActive);

        if (id.HasValue)
            query = query.Where(c => c.Id == id.Value);

        if (!string.IsNullOrEmpty(phoneNumber))
            query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));

        if (!string.IsNullOrEmpty(name))
            query = query.Where(c => c.Name.Contains(name));

        if (isActive.HasValue)
            query = query.Where(c => c.IsActive == isActive);

        if (isBloqued.HasValue)
            query = query.Where(c => c.IsBloqued == isBloqued);

        var totalCount = await query.CountAsync();
        var result = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new Tuple<List<Client>, int>(result, totalCount);
    }

    public async Task<int> CreateAsync(Client client)
    {
        _context.Clients.Add(client);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Client client)
    {
        _context.Entry(client).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)

    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client != null)
        {
            client.IsActive = false;
            _context.Entry(client).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        return 0;
    }

    // FIN ABM BASICO

    public async Task<List<Client>> GetUnblockedPhoneNumbers()
    {
        return await _context.Clients.Where(c => !c.IsBloqued).ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<Client?> GetByPhoneNumberAsync(string phone)
    {
        var query = _context.Clients.AsQueryable();
        query = query.Where(c => c.PhoneNumber == phone &&
                            c.IsActive &&
                            !c.IsBloqued);

        return await query.FirstOrDefaultAsync();
    }
}
