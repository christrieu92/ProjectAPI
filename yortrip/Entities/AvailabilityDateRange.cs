using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class AvailabilityDateRange
    {
        [Key]

        public DateTime StartRange { get; set; }

        public DateTime EndRange { get; set; }
    }
}
