using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Entities
{
    public class InviteeCalendar
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        public List<string> UnavailabilitiesDates { get; set; }
    }
}
