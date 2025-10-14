using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.Create;

public class CreateClientRequest : IRequest<ResponseModel<bool>>
{
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public CreateClientRequest(string phoneNumber, string name)
    {
        PhoneNumber = phoneNumber;
        Name = name;
    }
}