using BotWhatsapp.Application.Features.Service.Create;
using BotWhatsapp.Application.Features.Service.List;
using BotWhatsapp.Application.Features.Service.Update;
using BotWhatsapp.Application.Helpers;
using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.EntitiesVM;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;
using System.ComponentModel.Design;

namespace BotWhatsapp.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceRepository _repo;
    private readonly IAppointmentService _appointmentService;
    private readonly ICalendarService _calendarService;
    private readonly Interfaces.IResourceService _resourceService;

    public ServiceManager(IServiceRepository repo, IAppointmentService appointmentService, ICalendarService calendarService, Interfaces.IResourceService resourceService)
    {
        _repo = repo;
        _appointmentService = appointmentService;
        _calendarService = calendarService;
        _resourceService = resourceService;
    }

    public async Task<ResponseModel<PaginationResponse<ServiceDto>>> GetAllAsync(ServiceListRequest request)
    {
        try
        {
            var list = await _repo.GetAllAsync(request.PageNumber, request.PageSize, request.Id, request.Name);
            return new ResponseModel<PaginationResponse<ServiceDto>>()
            {
                Success = true,
                Data = new PaginationResponse<ServiceDto>
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = list.Item2,
                    Data = list.Item1
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<PaginationResponse<ServiceDto>>()
            {
                Success = false,
                Description = ex.Message
            };
        }
    }
    public async Task<ResponseModel<bool>> CreateAsync(CreateServiceRequest request)
    {
        try
        {
            var service = new Service
            {
                Name = request.Name,
                Price = request.Price,
                DurationInMinutes = request.DurationInMinutes,
                ResourceServices = request.ResourceIds.Select(uid => new ResourceService { ResourceId = uid }).ToList()
            };

            var created = await _repo.CreateAsync(service);

            return new ResponseModel<bool>
            {
                Success = created > 0,
                Data = created > 0,
                Description = created > 0 ? "Servicio creado exitosamente." : "Error en la creaci�n del servicio."
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al crear el servicio."
            };
        }
    }
    public async Task<ResponseModel<bool>> UpdateAsync(UpdateServiceRequest request)
    {
        try
        {
            var serviceU = new Service
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                DurationInMinutes = request.DurationInMinutes
            };

            var updated = await _repo.UpdateAsync(serviceU, request.ResourceIds);

            return new ResponseModel<bool>
            {
                Success = updated > 0,
                Data = updated > 0,
                Description = updated > 0 ? "Servicio actualizado exitosamente." : "Error al actualizar el servicio."
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al actualizar el servicio."
            };
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            var serviceD = await _repo.DeleteAsync(id);
            return new ResponseModel<bool>
            {
                Success = serviceD > 0,
                Data = serviceD > 0,
                Description = serviceD > 0 ? "Servicio eliminado exitosamente." : "Error al eliminar el servicio."
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al eliminar el servicio."
            };
        }
    }
    public async Task<ResponseModel<ServiceDto>?> GetByIdAsync(int id)
    {
        try
        {
            var s = await _repo.GetByIdAsync(id);
            return new ResponseModel<ServiceDto>()
            {
                Success = s != null,
                Data = s != null ? new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    DurationInMinutes = s.DurationInMinutes,
                    AssignedResourceIds = s.ResourceServices.Select(rs => rs.ResourceId).ToList()
                } : null,
                Description = s != null ? "Servicio encontrado." : "Servicio no encontrado."
            };
        }
        catch (Exception)
        {
            return new ResponseModel<ServiceDto>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el servicio."
            };

        }
    }

    public async Task<List<SlotInfo>> GetAvailableTimeSlotsAsync(int serviceId, string dateString)
    {
        var slotsWithResources = await GetAvailableTimeSlotsWithResourceAsync(serviceId, dateString);

        var groupedSlots = slotsWithResources
            .GroupBy(s => s.TimeSlot)
            .Select(g =>
            {
                var resourceIds = g.Select(s => s.ResourceId).ToList();
                var firstResourceId = resourceIds.First();
                var resourceInfo = resourceIds.Count > 1
                    ? $"(Cancha {string.Join(", ", resourceIds)})"
                    : $"(Cancha {firstResourceId})";

                return (g.Key, firstResourceId, resourceInfo);
            })
            .OrderBy(s => s.Key)
            .ToList();

        return groupedSlots.Select(y => new SlotInfo
        {
            ResourceId = y.firstResourceId,
            TimeSlot = y.Key,
            ResourceName = y.resourceInfo
        }).ToList();
    }

    public async Task<List<SlotInfo>> GetAvailableTimeSlotsWithResourceAsync(int serviceId, string dateString)
    {
        var availableSlots = new List<SlotInfo>();

        try
        {
            if (!DateTime.TryParse(dateString, out DateTime selectedDate))
            {
                return availableSlots;
            }

            var service = await GetByIdAsync(serviceId);
            if (!service.Success || service.Data == null)
            {
                return availableSlots;
            }

            var resourceIds = service.Data.AssignedResourceIds;
            if (!resourceIds.Any())
            {
                return availableSlots;
            }

            foreach (var resourceId in resourceIds)
            {
                var calendars = await _calendarService.GetAllCalendars();
                if (!calendars.Success || calendars.Data == null)
                    continue;

                var dayOfWeek = selectedDate.DayOfWeek;
                var calendarDay = calendars.Data.Find(c => c.Day == dayOfWeek && c.IsActive);
                if (calendarDay == null)
                    continue;

                var appointmentsResult = await GetResourceAppointmentsAsync(resourceId, selectedDate);
                var appointments = appointmentsResult?.Where(a => a.DateTime.Date == selectedDate.Date).ToList() ?? new List<Domain.Entities.Appointment>();

                var resourceSlots = AvailabilityHelper.GetAvailableSlots(calendarDay, appointments, service.Data.DurationInMinutes);

                var serviceDuration = service.Data.DurationInMinutes;
                var validSlots = FilterSlotsByDuration(resourceSlots, serviceDuration);
                var resource = await _resourceService.GetByIdAsync(resourceId);

                foreach (var slot in validSlots)
                {
                    var existingSlot = availableSlots.FirstOrDefault(s => s.TimeSlot == slot);

                    if (existingSlot == null)
                    {
                        // no existe todavía, se agrega uno nuevo
                        availableSlots.Add(new SlotInfo
                        {
                            TimeSlot = slot,
                            ResourceId = resourceId,
                            ResourceName = resource?.Data?.Name
                        });
                    }
                    else
                    {
                        // ya existe, concatenamos el nombre del recurso
                        if (!string.IsNullOrEmpty(resource?.Data?.Name)
                            && !existingSlot.ResourceName.Contains(resource.Data.Name))
                        {
                            existingSlot.ResourceName += $", {resource.Data.Name}";
                        }
                    }
                }

            }

            availableSlots = availableSlots.OrderBy(s => s.TimeSlot).ToList();

        }
        catch (Exception ex)
        {
            // Log del error si es necesario
        }

        return availableSlots;
    }

    private async Task<List<Appointment>> GetResourceAppointmentsAsync(int resourceId, DateTime date)
    {
        var result = await _appointmentService.GetByResourceIdAsync(resourceId);
        return result.Data?.Where(a => a.DateTime.Date == date.Date).ToList() ?? new List<Appointment>();
    }

    private List<string> FilterSlotsByDuration(List<string> slots, int requiredDurationMinutes)
    {
        var validSlots = new List<string>();

        foreach (var slot in slots)
        {
            var parts = slot.Split('-');
            if (parts.Length == 2 && TimeSpan.TryParse(parts[0], out TimeSpan startTime))
            {
                var endTime = startTime.Add(TimeSpan.FromMinutes(requiredDurationMinutes));
                validSlots.Add($@"{startTime:hh\:mm}-{endTime:hh\:mm}");
            }
        }

        return validSlots;
    }

    public async Task<ResponseModel<List<ServiceDto>>> GetServicesByResourceAsync(int userId)
    {
        try
        {
            var services = await _repo.GetServicesByResourceAsync(userId);
            return new ResponseModel<List<ServiceDto>>
            {
                Success = true,
                Data = services.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    DurationInMinutes = s.DurationInMinutes,
                    AssignedResourceIds = s.ResourceServices.Select(rs => rs.ResourceId).ToList()
                }).ToList() ?? [],
                Description = "Servicios obtenidos exitosamente."
            };
        }
        catch (Exception)
        {
            return new ResponseModel<List<ServiceDto>>
            {
                Success = false,
                Data = [],
                Description = "Error al obtener los servicios."
            };

        }
    }
}
