using BotWhatsapp.Application.Features.Resource.Create;
using BotWhatsapp.Application.Features.Resource.GetResource;
using BotWhatsapp.Application.Features.Resource.Update;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Services;

public class ResourcesService : IResourceService
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICalendarService _calendarService;

    public ResourcesService(IResourceRepository resourceRepository, IAppointmentRepository appointmentRepository, ICalendarService calendarService)
    {
        _resourceRepository = resourceRepository;
        _appointmentRepository = appointmentRepository;
        _calendarService = calendarService;
    }

    public async Task<ResponseModel<PaginationResponse<ResourceDto>>> GetAllAsync(ResourceListRequest request)
    {
        try
        {
            var resources = await _resourceRepository.GetAllAsync(request.IsActive, request.PageSize,
                                                                  request.PageNumber, request.Id, request.Name);
            return new ResponseModel<PaginationResponse<ResourceDto>>()
            {
                Success = true,
                Data = new PaginationResponse<ResourceDto>
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = resources.Item2,
                    Data = resources.Item1.Select(resource => new ResourceDto
                    {
                        Id = resource.Id,
                        Name = resource.Name,
                        Description = resource.Description,
                        Type = resource.Type,
                        IsActive = resource.IsActive,
                    }).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<PaginationResponse<ResourceDto>>()
            {
                Success = false,
                Description = ex.Message
            };
        }
    }

    public async Task<ResponseModel<PaginationResponse<ResourceDto>>> GetAllAsync()
    {
        try
        {
            var resources = await _resourceRepository.GetAllAsync();
            return new ResponseModel<PaginationResponse<ResourceDto>>()
            {
                Success = true,
                Data = new PaginationResponse<ResourceDto>
                {
                    TotalCount = resources.Item2,
                    Data = resources.Item1.Select(resource => new ResourceDto
                    {
                        Id = resource.Id,
                        Name = resource.Name,
                        Description = resource.Description,
                        Type = resource.Type,
                        IsActive = resource.IsActive,
                    }).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<PaginationResponse<ResourceDto>>()
            {
                Success = false,
                Description = ex.Message
            };
        }
    }

    public async Task<ResponseModel<bool>> CreateAsync(CreateResourceRequest request)
    {
        try
        {
            var resource = new Resource
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                IsActive = true
            };
            var res = await _resourceRepository.CreateResourceAsync(resource);

            await _resourceRepository.AddServicesToResourceAsync(res.Id, request.ServiceIds);

            // await _calendarService.CreateDefaultCalendarForResource(resource.Id); // Removed

            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Recurso y calendarios creados exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al crear el recurso/calendario"
            };
        }
    }
    public async Task<ResponseModel<bool>> UpdateAsync(UpdateResourceRequest request)
    {
        try
        {
            var resource = await _resourceRepository.GetByIdAsync(request.Id ?? 0);
            if (resource == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "Recurso no encontrado"
                };
            }

            resource.Name = request.Name;
            resource.Description = request.Description;
            resource.Type = request.Type;
            resource.IsActive = request.IsActive;

            await _resourceRepository.UpdateWithServicesAsync(resource, request.ServiceIds);
            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Recurso actualizado exitosamente"
            };

        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al actualizar el recurso"
            };
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            var appointmentsConfirmed = await _appointmentRepository.GetByResourceIdAsync(id);
            appointmentsConfirmed.Select(x => x.DateTime > DateTime.Now && x.Status == Domain.Enum.AppointmentStatus.Confirmed).ToList();
            if (appointmentsConfirmed.Any())
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "No se puede eliminar el recurso porque tiene turnos confirmados que no transcurrieron."
                };
            }
            await _resourceRepository.DeleteAsync(id);

            // await _calendarService.DeleteCalendarByResourceId(id); // Removed
            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Recurso eliminado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al eliminar el recurso"
            };
        }
    }
    public async Task<ResponseModel<ResourceDto?>> GetByIdAsync(int id)
    {
        try
        {
            var resource = await _resourceRepository.GetByIdAsync(id);
            var calendars = await _calendarService.GetAllCalendars();
            if (resource == null || calendars == null)
            {
                return new ResponseModel<ResourceDto?>
                {
                    Success = false,
                    Data = null,
                    Description = "Recurso no encontrado"
                };
            }

            return new ResponseModel<ResourceDto?>()
            {
                Success = true,
                Data = new ResourceDto()
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    Type = resource.Type,
                    Calendars = calendars.Data,
                    Description = resource.Description,
                    IsActive = resource.IsActive,
                },
            };
        }
        catch (Exception)
        {
            return new ResponseModel<ResourceDto?>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el recurso"
            };
        }
    }
    public async Task<ResponseModel<List<ServiceDto>>> GetServicesByResourceIdAsync(int resourceId)
    {
        try
        {
            var services = await _resourceRepository.GetServicesByResourceIdAsync(resourceId);
            if (services == null)
            {
                return new ResponseModel<List<ServiceDto>>
                {
                    Success = false,
                    Data = null,
                    Description = "Servicios no encontrados para el recurso"
                };
            }

            return new ResponseModel<List<ServiceDto>>
            {
                Success = true,
                Data = services.Select(x => new ServiceDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    DurationInMinutes = x.DurationInMinutes,
                    AssignedResourceIds = x.ResourceServices.Select(rs => rs.ResourceId).ToList()
                }).ToList(),
                Description = "Servicios obtenidos exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<List<ServiceDto>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener los servicios del recurso"
            };

        }
    }
    // public async Task<ResponseModel<List<CalendarDto>>> GetCalendarAsync(int resourceId) // Removed
    // {
    //     return await _calendarService.GetCalendarByResourceId(resourceId);
    // }
    // public async Task<ResponseModel<bool>> UpdateCalendarAsync(UpdateCalendarByResourceRequest request) // Removed
    // {
    //     return await _calendarService.UpdateCalendar(request.ResourceId ?? 0, request.Calendars);
    // }
}
