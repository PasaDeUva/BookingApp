using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BotWhatsapp.Infrastructure.Repository;

public class ResourceRepository : IResourceRepository
{
    private readonly AppDbContext _context;

    public ResourceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tuple<List<Resource>, int>> GetAllAsync(bool? isActive,
                                                              int pageSize = 10, int pageNumber = 1,
                                                              int? id = null, string? name = "")
    {
        var query = _context.Resources.AsQueryable();
        query = query.Where(e => e.IsActive);
        if (id.HasValue)
            query = query.Where(e => e.Id == id.Value);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(e => e.Name.Contains(name));
        if (isActive.HasValue)
            query = query.Where(e => e.IsActive == isActive.Value);

        var total = await query.CountAsync();
        var result = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new Tuple<List<Resource>, int>(result, total);
    }

    public async Task<Tuple<List<Resource>, int>> GetAllAsync()
    {
        var query = _context.Resources.AsQueryable();
        query = query.Where(e => e.IsActive);
        
        var total = await query.CountAsync();
        var result = await query
            .ToListAsync();

        return new Tuple<List<Resource>, int>(result, total);
    }

    public async Task<Resource?> GetByIdAsync(int id)
    {
        return await _context.Resources.Where(e => e.IsActive).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Service>> GetServicesByResourceIdAsync(int resourceId)
    {
        return await _context.ResourceServices.Where(es => es.ResourceId == resourceId).Select(es => es.Service).ToListAsync();
    }

    public async Task<Resource> CreateResourceAsync(Resource resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync(); // aquí se genera el Id si es identity/autoincrement
        return resource;
    }

    public async Task AddServicesToResourceAsync(int resourceId, List<int> serviceIds)
    {
        // Validar que el recurso existe
        var resourceExists = await _context.Resources.AnyAsync(r => r.Id == resourceId);
        if (!resourceExists)
            throw new InvalidOperationException($"No existe un recurso con Id {resourceId}");

        // Validar servicios existentes
        var validServiceIds = await _context.Services
                                    .Where(s => serviceIds.Contains(s.Id))
                                    .Select(s => s.Id)
                                    .ToListAsync();

        var resourceServices = validServiceIds.Select(serviceId => new ResourceService
        {
            ResourceId = resourceId,
            ServiceId = serviceId
        }).ToList();

        _context.ResourceServices.AddRange(resourceServices);
        await _context.SaveChangesAsync();
    }

    public async Task<Resource> UpdateWithServicesAsync(Resource resource, List<int> serviceIds)

    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Resources.Update(resource);

            var existingServices = await _context.ResourceServices.Where(es => es.ResourceId == resource.Id).ToListAsync();
            _context.ResourceServices.RemoveRange(existingServices);

            var newResourceServices = serviceIds.Select(serviceId => new ResourceService
            {
                ResourceId = resource.Id,
                ServiceId = serviceId
            });

            _context.ResourceServices.AddRange(newResourceServices);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return resource;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var resource = await GetByIdAsync(id);
        if (resource == null) return false;

        resource.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Resources.CountAsync();
    }
}