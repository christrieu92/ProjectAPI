using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class Notification
    {
        [Key]


        [Required]
        public Guid NotificationId { get; set; }

        public Guid CalendarId { get; set; }

        public Guid UserId { get; set; }

        public string Message { get; set; }

        public string Viewed { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
