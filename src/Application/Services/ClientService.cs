using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Response;
using BotWhatsapp.Domain.DTOs;
using BotWhatsapp.Application.Features.Client.GetAll;
using BotWhatsapp.Application.Mappers;

namespace BotWhatsapp.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<ResponseModel<PaginationResponse<ClientDto>>> GetAllAsync(GetAllClientRequest request)
    {
        try
        {
            var clients = await _clientRepository.GetAllAsync(request.Id, request.PhoneNumber, request.Name, request.PageNumber, request.PageSize, request.IsActive, request.IsBloqued);

            var clientDtos = clients.Item1.Select(client => ClientMapper.MapToClientDto(client)).ToList();

            return new ResponseModel<PaginationResponse<ClientDto>>()
            {
                Success = true,
                Data = new PaginationResponse<ClientDto>
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = clients.Item2,
                    Data = clientDtos
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<PaginationResponse<ClientDto>>()
            {
                Success = false,
                Description = ex.Message
            };

        }
    }
    public async Task<ResponseModel<bool>> CreateAsync(ClientCreateRequest request)
    {
        try
        {
            var client = new Client
            {
                PhoneNumber = request.PhoneNumber,
                Name = request.Name
            };
            await _clientRepository.CreateAsync(client);

            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Cliente creado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Description = "Error al crear el cliente"
            };
        }
    }
    public async Task<ResponseModel<bool>> UpdateAsync(int id, ClientUpdateRequest request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Description = "Cliente no encontrado"
                };
            }

            client.PhoneNumber = request.PhoneNumber;
            client.Name = request.Name;

            await _clientRepository.UpdateAsync(client);

            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Cliente actualizado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Description = "Error al actualizar el cliente"
            };
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            await _clientRepository.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Cliente eliminado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Description = "Error al eliminar el cliente"
            };
        }
    }
    public async Task<ResponseModel<ClientDto?>> GetByIdAsync(int id)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);

            return new ResponseModel<ClientDto?>()
            {
                Success = true,
                Data = client != null ? new ClientDto()
                {
                    Id = client.Id,
                    Name = client.Name,
                    PhoneNumber = client.PhoneNumber,
                    IsBloqued = client.IsBloqued
                } : null
            };
        }
        catch (Exception)
        {
            return new ResponseModel<ClientDto?>()
            {
                Success = false,
                Description = "Error al obtener el cliente"
            };
        }
    }
    public async Task<ResponseModel<ClientDto?>> GetByPhoneNumberAsync(string phone)
    {
        try
        {
            var client = await _clientRepository.GetByPhoneNumberAsync(phone);
            return new ResponseModel<ClientDto?>()

            {
                Success = true,
                Data = client != null ? new ClientDto()
                {
                    Id = client.Id,
                    Name = client.Name,
                    PhoneNumber = client.PhoneNumber,
                    IsBloqued = client.IsBloqued
                } : null
            };
        }
        catch (Exception)
        {
            return new ResponseModel<ClientDto?>()
            {
                Success = false,
                Description = "Error al obtener el cliente por nmero de telfono"
            };
        }
    }
    public async Task<ResponseModel<bool>> BlockAsync(int id, ClientBlockRequest request)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            client.IsBloqued = request.isBloqued;
            await _clientRepository.UpdateAsync(client);
            return new ResponseModel<bool>()
            {
                Success = true,
                Data = true,
                Description = request.isBloqued ? "Cliente bloqueado exitosamente" : "Cliente desbloqueado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Description = request.isBloqued ? "Error al bloquear el cliente" : "Error al desbloquear el cliente"
            };
        }
    }
}