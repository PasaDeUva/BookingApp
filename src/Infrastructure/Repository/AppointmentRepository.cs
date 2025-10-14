using BotWhatsapp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.EntitiesVM.Appointment;
using BotWhatsapp.Domain.Enum;

namespace BotWhatsapp.Infrastructure.Repository;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tuple<List<Appointment>, int>> GetAllAsync(AppointmentFilters request)

    {
        var query = _context.Appointments.Include(a => a.Client)
                             .Include(a => a.Service)
                             .Include(a => a.AssignedResource)
                             .Where(a => a.IsActive);

        var total = 0;

        if (request != null)
        {
            if (request.DateFrom.HasValue)
                query = query.Where(a => a.DateTime >= request.DateFrom.Value);

            if (request.DateTo.HasValue)
                query = query.Where(a => a.DateTime <= request.DateTo.Value);

            if (request.ResourceId.HasValue)
                query = query.Where(a => a.AssignedResourceId == request.ResourceId.Value);

            if (!string.IsNullOrEmpty(request.Status.ToString()))
                query = query.Where(a => a.Status == request.Status);

            if (!string.IsNullOrEmpty(request.ClientName))
                query = query.Where(a => a.Client.Name.Contains(request.ClientName));


            total = query.Count();
        }

        var items = await query
            .OrderByDescending(a => a.DateTime)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new Tuple<List<Appointment>, int>(items, total);
    }
    public async Task<int> CreateAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        return await _context.SaveChangesAsync();
    }
    public async Task<int> CancelAsync(int appointmentId)
    {
        var appt = await _context.Appointments
            .Where(a => a.Id == appointmentId)
            .FirstOrDefaultAsync();
        if (appt == null) return 0;

        appt.Status = AppointmentStatus.Cancelled;
        appt.IsActive = false;
        return await _context.SaveChangesAsync();
    }
    public async Task<List<Appointment>> GetUpcomingByClientIdAsync(int clientId)
    {
        var today = DateTime.Today;

        return await _context.Appointments
            .Where(a => a.ClientId == clientId && a.DateTime >= today && a.Status == AppointmentStatus.Confirmed)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }

    // Obtener el próximo turno de un cliente
    public async Task<Appointment?> GetNextAppointmentByClientIdAsync(int clientId)
    {
        var today = DateTime.Today;

        return await _context.Appointments
            .Where(a => a.ClientId == clientId && a.DateTime >= today && a.Status == AppointmentStatus.Confirmed)
            .OrderBy(a => a.DateTime)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateDateAsync(int appointmentId, DateTime newDate)

    {
        var appt = await _context.Appointments
            .Where(a => a.Id == appointmentId)
            .FirstOrDefaultAsync();
        if (appt == null) return;

        appt.DateTime = newDate;
        await _context.SaveChangesAsync();
    }


    public async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Appointment>> GetByClientIdAsync(int clientId)
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Include(a => a.AssignedResource)
            .Where(a => a.ClientId == clientId && a.DateTime > DateTime.MinValue).ToListAsync();
    }

    public async Task<List<Appointment>> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .Where(a => a.Client.PhoneNumber == phoneNumber && a.DateTime > DateTime.MinValue).ToListAsync();
    }

    public async Task<List<Appointment>> GetByResourceIdAsync(int resourceId)
    {
        return await _context.Appointments
            .Include(a => a.Service)
            .Include(a => a.AssignedResource)
            .Where(a => a.AssignedResourceId == resourceId && a.Status == AppointmentStatus.Confirmed)
            .OrderByDescending(a => a.DateTime)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByResourceIdBetweenDatesAsync(int userId, DateTime from, DateTime to)
    {
        return await _context.Appointments
            .Where(a => a.AssignedResourceId == userId &&
                        a.Status == AppointmentStatus.Confirmed &&
                        a.DateTime < to &&
                        a.DateTime.AddMinutes(a.Service.DurationInMinutes) > from)
            .Include(a => a.Service)
            .ToListAsync();
    }

    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _context.Appointments.AnyAsync(a => a.Code == code);
    }

    public async Task<Appointment?> GetByCodeAsync(int id)
    {
        return await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
