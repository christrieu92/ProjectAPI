using System;
using System.Collections.Generic;
using System.Globalization;
using yortrip.DbContexts;
using yortrip.Entities;


namespace yortrip.Services
{
    public class InviteRepository : IInviteRepository, IDisposable
    {
        private readonly CalenderContext _context;

        private UserRepository userRepository;

        private CalendarRepository calendarRepository;

        private UnavailabilityRepository unavailabilitiyRepository;

        public InviteRepository(CalenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CalendarModel GetCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            calendarRepository = new CalendarRepository(_context);

            return calendarRepository.GetCalendar(calendarId);
        }

        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            userRepository = new UserRepository(_context);

            return userRepository.GetUser(userId);
        }

        public void AddInviteCalendar(Guid calendarId, InviteeCalendar inviteeCalendar)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            if (inviteeCalendar == null)
            {
                throw new ArgumentNullException(nameof(inviteeCalendar));
            }

            // Add User
            userRepository = new UserRepository(_context);

            User user = new User();

            user = userRepository.GetUser(inviteeCalendar.Email);

            if (user == null)
            {
                user = new User
                {
                    Name = inviteeCalendar.Name,

                    Email = inviteeCalendar.Email,

                    CreateDate = DateTime.Now
                };

                userRepository.AddUser(user);

                _context.SaveChanges();
            }

            inviteeCalendar.UserId = user.UserId;

            Guid? userCalendar = userRepository.GetUserCalendar(user.UserId, calendarId);

            if (userCalendar == null || userCalendar == Guid.Empty)
            {
                // Add association user to calendar
                userRepository.AddUserCalendar(user.UserId, calendarId);
            }

            // Add unavailabilities to user and calendar
            List<DateTime> dates = new List<DateTime>();

            unavailabilitiyRepository = new UnavailabilityRepository(_context);

            inviteeCalendar.UnavailabilitiesDates.ForEach(x => dates.Add(DateTime.Parse(x, CultureInfo.InvariantCulture)));

            unavailabilitiyRepository.AddUnavailabilities(dates, user.UserId, calendarId);
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
