using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using yortrip.DbContexts;
using yortrip.Entities;


namespace yortrip.Services
{
    public class CalendarRepository : ICalendarRepository, IDisposable
    {
        private readonly CalenderContext _context;

        private UserRepository userRepository;

        private UnavailabilityRepository unavailabilitiyRepository;

        public CalendarRepository(CalenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void DeleteCalendar(CalendarModel calender)
        {
            if (calender == null)
            {
                throw new ArgumentNullException(nameof(calender));
            }

            _context.Calendars.Remove(calender);


        }

        public CalendarModel GetCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            return _context.Calendars.Where(c => c.CalendarId == calendarId).FirstOrDefault();
        }

        public IEnumerable<CalenadrEntity> GetCalendars(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var userCalendars = _context.UsersCalendar.Where(uc => uc.UserId == userId).ToList();

            List<CalenadrEntity> calendars = new List<CalenadrEntity>();

            foreach (var userCalendar in userCalendars)
            {
                CalendarModel calendar = _context.Calendars.Where(c => c.CalendarId == userCalendar.CalendarId).FirstOrDefault();

                CalenadrEntity calendarEntity = new CalenadrEntity
                {
                    CalendarId = calendar.CalendarId,

                    Name = calendar.Name,

                    Participants = _context.UsersCalendar.Where(c => c.CalendarId == userCalendar.CalendarId).AsEnumerable().Count(),

                    StartMonth = calendar.StartMonth,

                    EndMonth = calendar.EndMonth,

                    CreatedBy = calendar.CreatedBy,

                    CreatedByName = _context.Users.Where(u => u.UserId == calendar.CreatedBy).Select(x => x.Name).FirstOrDefault()
                };

                calendars.Add(calendarEntity);
            }

            return calendars;
        }

        public void AddCalendar(Guid userId, CalendarUnavailability calendarUnavailablity)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (calendarUnavailablity == null)
            {
                throw new ArgumentNullException(nameof(calendarUnavailablity));
            }

            // Add Calendar

            CalendarModel calendar = new CalendarModel
            {
                Name = calendarUnavailablity.Name,

                StartMonth = calendarUnavailablity.StartMonth,

                EndMonth = calendarUnavailablity.EndMonth,

                CreatedBy = userId,

                CreateDate = DateTime.Now
            };

            _context.Calendars.Add(calendar);

            _context.SaveChanges();

            Guid calendarId = calendar.CalendarId;

            calendarUnavailablity.CalendarId = calendarId;

            // Add association user to calendar

            userRepository = new UserRepository(_context);

            userRepository.AddUserCalendar(userId, calendarId);

            // Add unavailabilities to user and calendar

            List<DateTime> dates = new List<DateTime>();

            unavailabilitiyRepository = new UnavailabilityRepository(_context);

            calendarUnavailablity.UnavailabilitiesDates.ForEach(x => dates.Add(DateTime.Parse(x, CultureInfo.InvariantCulture)));

            unavailabilitiyRepository.AddUnavailabilities(dates, userId, calendarUnavailablity.CalendarId);
        }

        public void UpdateCalendar(CalendarModel calendar)
        {
            // no code in this implementation
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

    }
}
