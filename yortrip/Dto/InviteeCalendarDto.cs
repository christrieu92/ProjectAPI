using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace yortrip.Dto
{
    public class InviteeCalendarDto
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        public List<string> UnavailabilitiesDates { get; set; }
    }
}
