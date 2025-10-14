using BotWhatsapp.Domain.Dtos;
using BotWhatsapp.Domain.Entities;

namespace BotWhatsapp.Application.Helpers;

public static class AvailabilityHelper
{
    public static List<string> GetAvailableSlots(CalendarDto calendar, List<Appointment> appointments, int durationInMinutes)
    {
        var slots = new List<string>();
        if (!calendar.StartTime.HasValue || !calendar.EndTime.HasValue)
            return slots;
        var start = calendar.StartTime.Value;
        var end = calendar.EndTime.Value;
        if (end <= start)
            end = end.Add(TimeSpan.FromDays(1));
        var current = start;
        while (current.Add(TimeSpan.FromMinutes(durationInMinutes > 0 ? durationInMinutes : 30)) <= end)
        {
            var slotStart = current;
            var slotEnd = current.Add(TimeSpan.FromMinutes(durationInMinutes > 0 ? durationInMinutes : 30));
            bool overlaps = appointments.Any(a =>
                a.DateTime.TimeOfDay < slotEnd &&
                a.DateTime.AddMinutes(a.Service.DurationInMinutes).TimeOfDay > slotStart
            );
            if (!overlaps)
                slots.Add($"{slotStart:hh\\:mm}-{slotEnd:hh\\:mm}");
            current = current.Add(TimeSpan.FromMinutes(durationInMinutes > 0 ? durationInMinutes : 30));
        }
        return slots;
    }
}
