using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class UserCalendar
    {
        [Key]


        [Required]
        public Guid UserCalendarId { get; set; }

        public Guid UserId { get; set; }

        [Required]

        public Guid CalendarId { get; set; }
    }
}
