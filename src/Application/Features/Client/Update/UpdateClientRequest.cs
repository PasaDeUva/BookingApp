using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Update;

public class UpdateClientRequest : IRequest<ResponseModel<bool>>
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public UpdateClientRequest(int id, string phoneNumber, string name)
    {
        Id = id;
        PhoneNumber = phoneNumber;
        Name = name;
    }
}