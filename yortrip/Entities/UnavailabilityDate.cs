using System;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class UnavailabilityDate
    {
        [Key]

        public DateTime UnavailableDate { get; set; }
    }
}
