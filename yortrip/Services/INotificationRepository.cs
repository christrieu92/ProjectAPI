using System;
using System.Collections.Generic;
using yortrip.Entities;

namespace yortrip.Services
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotifications(Guid userId);

        Notification GetNotification(Guid notificationId);

        string GetNotificationKey(Guid userId);

        void AddNotification(Guid calendarId, Guid userId);

        void UpdateNotification(Guid userId, List<Notification> notifications, string notificationKey);

        void DeleteNotification(Notification notification);

        bool SaveChanges();
    }
}
