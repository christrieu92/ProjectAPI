using System;
using System.Collections.Generic;
using System.Linq;
using yortrip.DbContexts;
using yortrip.Entities;


namespace yortrip.Services
{
    public class NotificationRepository : INotificationRepository, IDisposable
    {
        private readonly CalenderContext _context;

        private UserRepository userRepository;

        private CalendarRepository calendarRepository;

        public NotificationRepository(CalenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public void DeleteNotification(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _context.Notifications.Remove(notification);
        }

        public Notification GetNotification(Guid notificationId)
        {
            if (notificationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(notificationId));
            }

            return _context.Notifications.Where(n => n.NotificationId == notificationId && n.Viewed == null).FirstOrDefault();
        }

        public IEnumerable<Notification> GetNotifications(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<Notification> notifications = (from n in _context.Notifications
                                                join c in _context.Calendars on n.CalendarId equals c.CalendarId
                                                select new
                                                {
                                                    Notificationid = n.NotificationId,
                                                    Calendarid = n.CalendarId,
                                                    Userid = n.UserId,
                                                    MessageInfo = n.Message,
                                                    ViewMessage = n.Viewed,
                                                    Createddate = n.CreatedDate,
                                                    Createdby = c.CreatedBy
                                                }).Where(x => x.Createdby == userId && x.ViewMessage == null).Select(n => new Notification
                                                {
                                                    NotificationId = n.Notificationid,
                                                    CalendarId = n.Calendarid,
                                                    UserId = n.Userid,
                                                    Message = n.MessageInfo,
                                                    Viewed = n.ViewMessage,
                                                    CreatedDate = n.Createddate
                                                }).ToList();

            return notifications;
        }

        public string GetNotificationKey(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var notifcationKey = (from n in _context.Notifications
                                  join c in _context.Calendars on n.CalendarId equals c.CalendarId
                                  select new
                                  {
                                      Notificationid = n.NotificationId,
                                      Calendarid = n.CalendarId,
                                      Userid = n.UserId,
                                      MessageInfo = n.Message,
                                      ViewMessage = n.Viewed,
                                      Createddate = n.CreatedDate,
                                      Createdby = c.CreatedBy
                                  }).Where(x => x.Createdby == userId).OrderByDescending(nd => nd.Createddate).Select(n =>
                                      n.ViewMessage
                                     ).FirstOrDefault();

            return notifcationKey;
        }

        public void ReadNotifications(List<Guid> notificationsId)
        {
            _context.Notifications.Where(n => notificationsId.Contains(n.NotificationId)).ToList();

            return;
        }

        public void AddNotification(Guid calendarId, Guid userId)
        {
            if (calendarId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(calendarId));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            userRepository = new UserRepository(_context);

            calendarRepository = new CalendarRepository(_context);

            Notification notification = new Notification
            {
                CalendarId = calendarId,

                UserId = userId,

                Message = userRepository.GetUser(userId).Name + " has added their dates to " + calendarRepository.GetCalendar(calendarId).Name,

                Viewed = null,

                CreatedDate = DateTime.Now
            };

            _context.Notifications.Add(notification);
        }

        public void UpdateNotification(Guid userId, List<Notification> notifications, string notificationKey)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (notifications == null)
            {
                throw new Exception("No Notification Found");
            }

            foreach (var notification in notifications)
            {
                Notification result = GetNotification(notification.NotificationId);

                result.Viewed = notificationKey;

                _context.SaveChanges();
            }
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
