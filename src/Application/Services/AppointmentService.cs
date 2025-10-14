using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Events;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.EntitiesVM.Appointment;
using BotWhatsapp.Domain.Response;
using BotWhatsapp.Domain.Enum;

namespace BotWhatsapp.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;
    private readonly IServiceRepository _serviceRepo;
    private readonly IClientRepository _clientRepo;
    private readonly ICalendarRepository _calendarRepository;
    private readonly IResourceRepository _resourceRepo;
    private readonly IEventService _eventManager;

    public AppointmentService(
        IAppointmentRepository repo,
        IServiceRepository serviceRepo,
        IClientRepository clientRepo,
        ICalendarRepository calendarRepository,
        IEventService eventManager,
        IResourceRepository resourceRepo)
    {
        _repo = repo;
        _serviceRepo = serviceRepo;
        _clientRepo = clientRepo;
        _resourceRepo = resourceRepo;
        _calendarRepository = calendarRepository;
        _eventManager = eventManager;
    }

    public async Task<ResponseModel<PaginationResponse<Appointment>>> GetAllAsync(AppointmentFilters filters)
    {
        try
        {
            var items = await _repo.GetAllAsync(filters);
            return new ResponseModel<PaginationResponse<Appointment>>
            {
                Success = true,
                Data = new PaginationResponse<Appointment>
                {
                    Data = items.Item1,
                    TotalCount = items.Item2,
                    PageSize = filters.PageSize,
                    PageNumber = filters.PageNumber
                },
                Description = "Turnos obtenidos exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<PaginationResponse<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener los turnos"
            };
        }

    }
    public async Task<ResponseModel<bool>> CreateAsync(CreateAppointmentRequest request)
    {
        try
        {
            var response = new ResponseModel<bool>()
            {
                Success = true,
                Data = true,
                Description = "Turno creado exitosamente"
            };

            // Validar cliente
            var client = await _clientRepo.GetByIdAsync(request.ClientId);
            if (client == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Cliente no encontrado";
                return response;
            }
            // Validar servicio
            var service = await _serviceRepo.GetByIdAsync(request.ServiceId);
            if (service == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Servicio no encontrado";
                return response;
            }
            // Validar fecha
            if (request.DateTime < DateTime.Now)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "La fecha del turno debe ser futura";
                return response;
            }
            // Validar empleado y disponibilidad si se asigna uno
            if (request.AssignedResourceId.HasValue)
            {
                var resource = await _resourceRepo.GetByIdAsync(request.AssignedResourceId.Value);
                if (resource == null)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "Empleado no encontrado";
                    return response;
                }
                ;
                // Verificar que el empleado puede realizar el servicio
                var canProvideService = service.ResourceServices
                    .Any(es => es.ResourceId == request.AssignedResourceId.Value);
                if (!canProvideService)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "El recurso no está habilitado para este servicio";
                    return response;
                }
                // Verificar disponibilidad
                var isAvailable = await IsResourceAvailable(
                    request.AssignedResourceId.Value,
                    request.DateTime,
                    service.DurationInMinutes);

                if (!isAvailable.Success)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "El recurso no está disponible en ese horario";
                    return response;
                }
            }

            var appointment = new Appointment
            {
                ClientId = request.ClientId,
                ServiceId = request.ServiceId,
                DateTime = request.DateTime,
                Status = AppointmentStatus.Confirmed,
                AssignedResourceId = request.AssignedResourceId,
                CreatedAt = DateTime.Now,
                Code = await GenerateUniqueCodeAsync()
            };

            var appC = await _repo.CreateAsync(appointment);

            if (appC > 0)
            {
                await _eventManager.CreateAsync(new Event
                {
                    AppointmentCode = appointment.Code,
                    Description = AppointmentEvents.APPOINTMENT_CREATED,
                });
            }

            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Turno creado exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al crear el turno"
            };
        }

    }
    public async Task<ResponseModel<bool>> UpdateAsync(UpdateAppointmentRequest request)
    {
        var response = new ResponseModel<bool>
        {
            Success = true,
            Data = false,
            Description = "Turno actualizado exitosamente"
        };
        try
        {
            var appointment = await _repo.GetByIdAsync(request.Id);
            if (appointment == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Turno no encontrado";
                return response;
            }
            var service = await _serviceRepo.GetByIdAsync(request.ServiceId);
            if (service == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Servicio no encontrado";
                return response;
            }
            if (request.AssignedResourceId.HasValue)
            {
                var disponible = await IsResourceAvailable(
                    request.AssignedResourceId.Value,
                    request.DateTime,
                    service.DurationInMinutes,
                    request.Id
                );
                if (!disponible.Success)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "El recurso no está disponible en ese horario";
                    return response;
                }
            }
            if (request.AssignedResourceId.HasValue)
            {
                var assignedOk = service.ResourceServices.Any(us => us.ResourceId == request.AssignedResourceId.Value);
                if (!assignedOk)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "El recurso no está habilitado para este servicio";
                    return response;
                }
            }

            appointment.DateTime = request.DateTime;
            appointment.ServiceId = request.ServiceId;
            appointment.Status = request.Status;
            appointment.AssignedResourceId = request.AssignedResourceId;

            var appU = await _repo.UpdateAsync(appointment);

            return new ResponseModel<bool>
            {
                Success = appU > 0,
                Data = appU > 0,
                Description = appU > 0 ? "Turno actualizado exitosamente" : "Error al actualizar el turno"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al actualizar el turno"
            };
            throw;
        }

    }
    public async Task<ResponseModel<bool>> UpdateByDateAsync(UpdateAppointmentByDateRequest request)
    {
        var response = new ResponseModel<bool>
        {
            Success = true,
            Data = false,
            Description = "Turno actualizado exitosamente"
        };
        try
        {
            var appointment = await _repo.GetByIdAsync(request.Id);
            if (appointment == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Turno no encontrado";
                return response;
            }
            var service = await _serviceRepo.GetByIdAsync(appointment.ServiceId);
            if (service == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Servicio no encontrado";
                return response;
            }
            if (appointment.AssignedResourceId.HasValue)
            {
                var disponible = await IsResourceAvailable(
                    appointment.AssignedResourceId.Value,
                    request.Date,
                    service.DurationInMinutes,
                    request.Id
                );
                if (!disponible.Success)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = disponible.Description ?? "El recurso no está disponible en ese horario";
                    return response;
                }
            }
            if (appointment.AssignedResourceId.HasValue)
            {
                var assignedOk = service.ResourceServices.Any(us => us.ResourceId == appointment.AssignedResourceId.Value);
                if (!assignedOk)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Description = "El recurso no está habilitado para este servicio";
                    return response;
                }
            }

            appointment.DateTime = request.Date;
            appointment.ServiceId = appointment.ServiceId;
            appointment.Status = appointment.Status;
            appointment.AssignedResourceId = appointment.AssignedResourceId;

            var appU = await _repo.UpdateAsync(appointment);

            return new ResponseModel<bool>
            {
                Success = appU > 0,
                Data = appU > 0,
                Description = appU > 0 ? "Turno actualizado exitosamente" : "Error al actualizar el turno"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al actualizar el turno"
            };
            throw;
        }
    }
    public async Task<ResponseModel<bool>> DeleteAsync(int id)
    {
        try
        {
            var appC = await _repo.CancelAsync(id);
            return new ResponseModel<bool>
            {
                Success = appC > 0,
                Data = appC > 0,
                Description = appC > 0 ? "Turno cancelado exitosamente" : "Error al cancelar el turno"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al cancelar el turno"
            };
        }
    }
    public async Task<ResponseModel<bool>> IsResourceAvailable(int resourceId, DateTime dateTime, int duration)
    {
        try
        {
            var response = new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "Recurso disponible"
            };

            // Validar que el recurso exista
            var resource = await _resourceRepo.GetByIdAsync(resourceId);
            if (resource == null)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "Recurso no encontrado";
                return response;
            }

            // Validar que la fecha sea futura
            if (dateTime < DateTime.Now)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "La fecha debe ser futura";
                return response;
            }

            // Validar duración
            if (duration <= 0)
            {
                response.Success = false;
                response.Data = false;
                response.Description = "La duración debe ser mayor a 0";
                return response;
            }
            var endTime = dateTime.AddMinutes(duration);

            // Obtener todos los turnos del recurso para ese día
            var appointments = await _repo.GetByResourceIdAsync(resourceId);
            var dayAppointments = appointments.Where(a => a.DateTime.Date == dateTime.Date && a.Status != AppointmentStatus.Cancelled).ToList();

            // Verificar si hay solapamiento con otros turnos
            foreach (var appointment in dayAppointments)
            {
                var existingStart = appointment.DateTime;
                var existingEnd = existingStart.AddMinutes(appointment.Service.DurationInMinutes);

                if (!(endTime <= existingStart || dateTime >= existingEnd))
                {
                    return new ResponseModel<bool>
                    {
                        Success = false,
                        Data = false,
                        Description = "El recurso no está disponible en ese horario"
                    };
                }
            }
            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Description = "El recurso está disponible en ese horario"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al verificar la disponibilidad del recurso"
            };
        }
    }
    public async Task<ResponseModel<Appointment>> GetByIdAsync(int id)
    {
        try
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null) return new ResponseModel<Appointment>
            {
                Success = false,
                Data = null,
                Description = "Turno no encontrado"
            };

            return new ResponseModel<Appointment>
            {
                Success = true,
                Data = appointment,
                Description = "Turno obtenido exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<Appointment>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el turno"
            };
        }
    }

    public async Task<ResponseModel<List<Appointment>>> GetByClientIdAsync(int clientId)
    {
        try
        {
            var appointments = await _repo.GetByClientIdAsync(clientId);
            if (appointments == null) return new ResponseModel<List<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Turno no encontrado"
            };

            return new ResponseModel<List<Appointment>>
            {
                Success = true,
                Data = appointments,
                Description = "Turno obtenido exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<List<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el turno"
            };
        }
    }

    public async Task<ResponseModel<List<Appointment>>> GetByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var appointments = await _repo.GetByPhoneNumberAsync(phoneNumber);
            if (appointments == null) return new ResponseModel<List<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Turno no encontrado"
            };

            return new ResponseModel<List<Appointment>>
            {
                Success = true,
                Data = appointments,
                Description = "Turno obtenido exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<List<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el turno"
            };
        }
    }

    public async Task<ResponseModel<List<Appointment>>> GetByResourceIdAsync(int resourceId)
    {
        try
        {
            var apps = await _repo.GetByResourceIdAsync(resourceId);
            return new ResponseModel<List<Appointment>>
            {
                Success = true,
                Data = apps,
                Description = "Turnos obtenidos exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<List<Appointment>>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener los turnos del recurso"
            };
        }
    }
    private async Task<ResponseModel<bool>> IsResourceAvailable(int userId, DateTime startTime, int durationMinutes, int? excludeAppointmentId = null)
    {
        try
        {

            var endTime = startTime.AddMinutes(durationMinutes);
            var appointments = await _repo.GetByResourceIdAsync(userId);
            var calendars = await _calendarRepository.GetAllAsync();
            var calendarDay = calendars.FirstOrDefault(c => c.Day == startTime.DayOfWeek && c.IsActive);
            if (calendarDay == null || !calendarDay.StartTime.HasValue || !calendarDay.EndTime.HasValue)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "El recurso no tiene calendario activo para ese día"
                };
            }
            if (startTime.TimeOfDay < calendarDay.StartTime.Value || endTime.TimeOfDay > calendarDay.EndTime.Value)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                    Description = "El turno está fuera del horario permitido por el calendario"
                };
            }
            if (excludeAppointmentId.HasValue)
            {
                appointments = appointments.Where(a => a.Id != excludeAppointmentId.Value).ToList();
            }

            return new ResponseModel<bool>
            {
                Success = IsAvailable(appointments, startTime, endTime),
                Data = IsAvailable(appointments, startTime, endTime),
                Description = IsAvailable(appointments, startTime, endTime) ? "El recurso está disponible" : "El recurso no está disponible"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Data = false,
                Description = "Error al verificar la disponibilidad del recurso"
            };
        }
    }
    private bool IsAvailable(List<Appointment> appointments, DateTime startTime, DateTime endTime)
    {
        return appointments.All(a =>
        {
            var existingStart = a.DateTime;
            var existingEnd = a.DateTime.AddMinutes(a.Service.DurationInMinutes);
            return existingEnd <= startTime || existingStart >= endTime;
        });
    }

    /// <summary>
    /// Genera un código único verificando que no exista en la base de datos.
    /// </summary>
    /// <param name="length">Longitud del código</param>
    /// <returns>Código único</returns>
    private async Task<string> GenerateUniqueCodeAsync(int length = 10)
    {
        string code;
        bool exists;

        do
        {
            code = GenerateRandomCode(length);
            // Verificar si el código ya existe (necesitarías agregar este método al repositorio)
            exists = await _repo.CodeExistsAsync(code);
        } while (exists);

        return code;
    }

    /// <summary>
    /// Genera un código aleatorio criptográficamente seguro de la longitud especificada.
    /// </summary>
    /// <param name="length">Longitud del código a generar</param>
    /// <returns>Código aleatorio</returns>
    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new char[length];

        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[bytes[i] % chars.Length];
            }
        }

        return new string(result);
    }

    public async Task<ResponseModel<Appointment>> GetByCodeAsync(int code)
    {
        try
        {
            var appointment = await _repo.GetByCodeAsync(code);
            if (appointment == null) return new ResponseModel<Appointment>
            {
                Success = false,
                Data = null,
                Description = "Turno no encontrado"
            };

            return new ResponseModel<Appointment>
            {
                Success = true,
                Data = appointment,
                Description = "Turno obtenido exitosamente"
            };
        }
        catch (Exception)
        {
            return new ResponseModel<Appointment>
            {
                Success = false,
                Data = null,
                Description = "Error al obtener el turno"
            };
        }
    }
}