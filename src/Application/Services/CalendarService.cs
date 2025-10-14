using BotWhatsapp.Application.Interfaces;
using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;
using BotWhatsapp.Domain.Enums;
using BotWhatsapp.Domain.Interfaces;
using BotWhatsapp.Domain.Response;

namespace BotWhatsapp.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly ICalendarRepository _calendarRepository;

    public CalendarService(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<ResponseModel<bool>> CreateCalendar(List<DayOfWeek> days, TimeSpan startTime, TimeSpan endTime, int ownerId, CalendarOwnerType ownerType)
    {
        try
        {
            // Clear existing calendars for the specified owner
            var existingCalendars = await _calendarRepository.GetByOwnerAsync(ownerId, ownerType);
            foreach (var calendar in existingCalendars)
            {
                await _calendarRepository.DeleteAsync(calendar.Id);
            }

            foreach (var day in days)
            {              

                var calendar = new Calendar
                {
                    Day = day,
                    StartTime = startTime,
                    EndTime = endTime,
                    IsActive = true, // Set as active as per requirement
                    OwnerId = ownerId,
                    OwnerType = ownerType
                };
                await _calendarRepository.CreateAsync(calendar);
            }

            return new ResponseModel<bool> { Success = true, Data = true, Description = "Calendar created successfully." };
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool> { Success = false, Data = false, Description = $"Error creating calendar: {ex.Message}" };
        }
    }

    public async Task<ResponseModel<bool>> UpdateCalendar(CalendarDto calendarDto)
    {
        try
        {
            var calendar = await _calendarRepository.GetByIdAsync(calendarDto.Id);
            if (calendar == null)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Description = "Calendar entry not found." };
            }

            calendar.Day = calendarDto.Day;
            calendar.StartTime = calendarDto.StartTime;
            calendar.EndTime = calendarDto.EndTime;
            calendar.IsActive = calendarDto.IsActive;
            calendar.OwnerId = calendarDto.OwnerId;
            calendar.OwnerType = calendarDto.OwnerType;

            await _calendarRepository.UpdateAsync(calendar);
            return new ResponseModel<bool> { Success = true, Data = true, Description = "Calendar entry updated successfully." };
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool> { Success = false, Data = false, Description = $"Error updating calendar entry: {ex.Message}" };
        }
    }

    public async Task<ResponseModel<List<CalendarDto>>> GetAllCalendars()
    {
        try
        {
            var calendars = await _calendarRepository.GetAllAsync();
            var calendarDtos = calendars.Select(c => new CalendarDto
            {
                Id = c.Id,
                Day = c.Day,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                IsActive = c.IsActive,
                OwnerId = c.OwnerId,
                OwnerType = c.OwnerType
            }).ToList();

            return new ResponseModel<List<CalendarDto>>
            {
                Success = true,
                Data = calendarDtos,
                Description = "Calendars retrieved successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<List<CalendarDto>>
            {
                Success = false,
                Data = null,
                Description = $"Error retrieving calendars: {ex.Message}"
            };
        }
    }

    public async Task<ResponseModel<CalendarDto>> GetCalendarByDay(DayOfWeek day)
    {
        try
        {
            var c = await _calendarRepository.GetByDayAsync(day);
            var calendarDto =  new CalendarDto
            {
                Id = c.Id,
                Day = c.Day,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                IsActive = c.IsActive,
                OwnerId = c.OwnerId,
                OwnerType = c.OwnerType
            };

            return new ResponseModel<CalendarDto>
            {
                Success = true,
                Data = calendarDto,
                Description = "Calendars retrieved successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<CalendarDto>
            {
                Success = false,
                Data = null,
                Description = $"Error retrieving calendars: {ex.Message}"
            };
        }
    }

    public async Task<ResponseModel<CalendarDto>> GetCalendarById(int id)
    {
        try
        {
            var calendar = await _calendarRepository.GetByIdAsync(id);
            if (calendar == null)
            {
                return new ResponseModel<CalendarDto> { Success = false, Data = null, Description = "Calendar entry not found." };
            }

            var calendarDto = new CalendarDto
            {
                Id = calendar.Id,
                Day = calendar.Day,
                StartTime = calendar.StartTime,
                EndTime = calendar.EndTime,
                IsActive = calendar.IsActive,
                OwnerId = calendar.OwnerId,
                OwnerType = calendar.OwnerType
            };

            return new ResponseModel<CalendarDto> { Success = true, Data = calendarDto, Description = "Calendar entry retrieved successfully." };
        }
        catch (Exception ex)
        {
            return new ResponseModel<CalendarDto> { Success = false, Data = null, Description = $"Error retrieving calendar entry: {ex.Message}" };
        }
    }

    public async Task<ResponseModel<bool>> DeleteCalendar(int id)
    {
        try
        {
            var rowsAffected = await _calendarRepository.DeleteAsync(id);
            if (rowsAffected == 0)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Description = "Calendar entry not found or could not be deleted." };
            }
            return new ResponseModel<bool> { Success = true, Data = true, Description = "Calendar entry deleted successfully." };
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool> { Success = false, Data = false, Description = $"Error deleting calendar entry: {ex.Message}" };
        }
    }
}
