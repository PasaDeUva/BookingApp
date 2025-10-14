using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.Response;
using MediatR;

namespace BotWhatsapp.Application.Features.Client.GetById
{
    public class GetClientByIdRequest : IRequest<ResponseModel<ClientDto?>>
    {
        public int Id { get; set; }
        
        public GetClientByIdRequest(int id)
        {
            Id = id;
        }
    }
}