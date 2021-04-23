using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace yortrip.Entities
{
    public class CalendarModel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CalendarId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime StartMonth { get; set; }

        public DateTime EndMonth { get; set; }

        public Guid CreatedBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
