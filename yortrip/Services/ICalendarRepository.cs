using System;
using System.Collections.Generic;
using yortrip.Entities;

namespace yortrip.Services
{
    public interface ICalendarRepository
    {
        IEnumerable<CalenadrEntity> GetCalendars(Guid userId);

        CalendarModel GetCalendar(Guid calendarId);

        void AddCalendar(Guid userId, CalendarUnavailability calendar);

        void UpdateCalendar(CalendarModel calendar);

        void DeleteCalendar(CalendarModel calendar);

        bool SaveChanges();
    }
}
