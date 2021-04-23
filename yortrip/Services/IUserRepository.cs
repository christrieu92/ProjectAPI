using System;
using System.Collections.Generic;
using yortrip.Entities;

namespace yortrip.Services
{
    public interface IUserRepositroy
    {
        User GetUser(string email);

        User GetUser(Guid userId);

        public List<User> GetUsersByCalendar(Guid calendarId);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(User user);

        void AddUserCalendar(Guid userId, Guid calendarId);

        Guid? GetUserCalendar(Guid userId, Guid calendarId);

        bool SaveChanges();
    }
}
