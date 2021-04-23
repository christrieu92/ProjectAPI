using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class Unavailability
    {
        [Key]


        [Required]
        public Guid UnavailabilityId { get; set; }

        public Guid UserId { get; set; }

        public Guid CalendarId { get; set; }

        public DateTime UnavailabilityDate { get; set; }
    }
}
