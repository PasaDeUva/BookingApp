using BotWhatsapp.Application.Features.Client.GetAll;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Interfaces;

public interface IClientService
{
    Task<ResponseModel<PaginationResponse<ClientDto>>> GetAllAsync(GetAllClientRequest request);
    Task<ResponseModel<bool>> CreateAsync(ClientCreateRequest request);
    Task<ResponseModel<bool>> UpdateAsync(int id, ClientUpdateRequest request);
    Task<ResponseModel<bool>> DeleteAsync(int id);
    Task<ResponseModel<ClientDto?>> GetByIdAsync(int id);
    Task<ResponseModel<ClientDto?>> GetByPhoneNumberAsync(string phone);
    Task<ResponseModel<bool>> BlockAsync(int id, ClientBlockRequest request);
}