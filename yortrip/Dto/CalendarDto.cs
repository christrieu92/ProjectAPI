using System;

namespace yortrip.Dto
{
    public class CalendarDto
    {
        public Guid CalendarId { get; set; }

        public string Name { get; set; }

        public int Participants { get; set; }

        public DateTime StartMonth { get; set; }

        public DateTime EndMonth { get; set; }

        public Guid CreatedBy { get; set; }

        public string CreatedByName { get; set; }

    }
}
