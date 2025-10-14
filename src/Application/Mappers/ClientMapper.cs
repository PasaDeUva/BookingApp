using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Application.Mappers;

public class ClientMapper
{
    public static ClientDto MapToClientDto(Client client)
    {
        if (client == null) return null;

        return new ClientDto
        {
            Id = client.Id,
            PhoneNumber = client.PhoneNumber,
            Name = client.Name,
            IsActive = client.IsActive,
            IsBloqued = client.IsBloqued
        };
    }

}
