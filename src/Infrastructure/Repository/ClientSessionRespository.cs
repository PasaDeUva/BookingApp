using BotWhatsapp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BotWhatsapp.Infrastructure.Context;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Enums;

namespace BotWhatsapp.Infrastructure.Repository;

public class ClientSessionRepository : IClientSessionRepository
{
    private readonly AppDbContext _context;

    public ClientSessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClientSession> GetOrCreateAsync(string phone)
    {
        var session = await _context.ClientSessions.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
        if (session == null)
        {
            session = new ClientSession { PhoneNumber = phone };
            _context.ClientSessions.Add(session);
            await _context.SaveChangesAsync();
        }
        return session;
    }

    public async Task UpdateAsync(ClientSession session)
    {
        _context.ClientSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWithStepAsync(ClientSession session, ClientStep? currentStep = null)
    {
        session.CurrentStep = currentStep.Value;
        _context.ClientSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string phone)
    {
        var session = await _context.ClientSessions.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
        if (session != null)
        {
            _context.ClientSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }
}
