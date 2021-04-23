using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Dto
{
    public class CalendarCreateDto
    {
        [Required]
        public string Name { get; set; }

        public DateTime StartMonth { get; set; }

        public DateTime EndMonth { get; set; }

        [Required]
        public List<string> UnavailabilitiesDates { get; set; }
    }
}
