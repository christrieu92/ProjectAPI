using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class AvailabilityDate
    {
        [Key]

        public DateTime AvailableDate { get; set; }
    }
}
