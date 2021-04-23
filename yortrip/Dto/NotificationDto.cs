using System;

namespace yortrip.Dto
{
    public class NotificationDto
    {

        public Guid NotificationId { get; set; }

        public Guid CalendarId { get; set; }

        public Guid UserId { get; set; }

        public string Message { get; set; }

        public bool Viewed { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
