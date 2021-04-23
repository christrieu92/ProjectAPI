using System;
using yortrip.Entities;


namespace yortrip.Services
{
    public interface IInviteRepository
    {
        CalendarModel GetCalendar(Guid calendarId);

        User GetUser(Guid userId);

        void AddInviteCalendar(Guid calendarId, InviteeCalendar inviteeCalendar);

        bool SaveChanges();
    }
}
