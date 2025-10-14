using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetByPhone
{
    public class GetClientByPhoneRequest : IRequest<ResponseModel<ClientDto?>>
    {
        public string Phone { get; set; }
        
        public GetClientByPhoneRequest(string phone)
        {
            Phone = phone;
        }
    }
}