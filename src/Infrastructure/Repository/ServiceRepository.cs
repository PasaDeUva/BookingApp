using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;
using Microsoft.EntityFrameworkCore;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Domain.Interfaces;

namespace BotWhatsapp.Infrastructure.Repository;

public class ServiceRepository : IServiceRepository
{

    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Tuple<List<ServiceDto>, int>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? id = null, string? name = "")
    {
        IQueryable<Service> query = _context.Services.Include(s => s.ResourceServices);

        if (id.HasValue)
            query = query.Where(c => c.Id == id.Value);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(c => c.Name.Contains(name));

        var totalCount = await query.CountAsync();
        var result = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                DurationInMinutes = s.DurationInMinutes,
                AssignedResourceIds = s.ResourceServices.Select(rs => rs.ResourceId).ToList()
            })
            .ToListAsync();

        return new Tuple<List<ServiceDto>, int>(result, totalCount);
    }
    public async Task<int> CreateAsync(Service service)
    {
        _context.Services.Add(service);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> UpdateAsync(Service service, List<int> resourcesId)
    {
        var existing = await _context.Services
                       .Include(s => s.ResourceServices).FirstOrDefaultAsync(s => s.Id == service.Id);

        if (existing == null)
        {
            return 0;
        }

        existing.Name = service.Name;
        existing.Price = service.Price;
        existing.DurationInMinutes = service.DurationInMinutes;

        existing.ResourceServices.Clear();
        foreach (var id in resourcesId)
        {
            existing.ResourceServices.
                Add(new ResourceService { ResourceId = id, ServiceId = existing.Id });
        }

        return await _context.SaveChangesAsync();
    }
    public async Task<int> DeleteAsync(int id)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == id);
        if (service != null)
        {
            _context.Services.Remove(service);
            return await _context.SaveChangesAsync();
        }
        return 0;
    }
    public async Task<Service?> GetByIdAsync(int id)
    {
        return await _context.Services.Include(s => s.ResourceServices).ThenInclude(us => us.Resource)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<List<Service>> GetServicesByResourceAsync(int userId)
    {
        return await _context.Services
            .Include(s => s.ResourceServices)
            .Where(s => s.ResourceServices.Any(us => us.ResourceId == userId))
            .ToListAsync();
    }
}
