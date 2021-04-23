using System;
using System.Collections.Generic;
using System.Linq;
using yortrip.DbContexts;
using yortrip.Entities;

namespace yortrip.Services
{
    public class UserRepository : IUserRepositroy, IDisposable
    {
        private readonly CalenderContext _context;

        public UserRepository(CalenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Remove(user);
        }

        public User GetUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return _context.Users.Where(c => c.Email == email).FirstOrDefault();
        }

        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.Where(c => c.UserId == userId).FirstOrDefault();
        }

        public List<User> GetUsersByCalendar(Guid calendarId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            var userList = (from uc in _context.UsersCalendar
                            join u in _context.Users on uc.UserId equals u.UserId
                            select new
                            {
                                Calendarid = uc.CalendarId,
                                Userid = u.UserId,
                                UserName = u.Name,

                            }).Where(x => x.Calendarid == calendarId).Select(x => new User
                            {
                                UserId = x.Userid,
                                Name = x.UserName
                            }).ToList();

            return userList;
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
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

        public void AddUserCalendar(Guid userId, Guid calendarId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            UserCalendar userCalendar = new UserCalendar
            {
                UserId = userId,
                CalendarId = calendarId
            };

            _context.UsersCalendar.Add(userCalendar);
        }

        public Guid? GetUserCalendar(Guid userId, Guid calendarId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            return _context.UsersCalendar.Where(uc => uc.UserId == userId && uc.CalendarId == calendarId).Select(uc => uc.UserCalendarId).FirstOrDefault();
        }
    }
}
