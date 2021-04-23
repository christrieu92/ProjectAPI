using Microsoft.EntityFrameworkCore;
using yortrip.Entities;

namespace yortrip.DbContexts
{
    public class CalenderContext : DbContext
    {
        public CalenderContext(DbContextOptions<CalenderContext> options)
           : base(options)
        {
        }

        public DbSet<CalendarModel> Calendars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCalendar> UsersCalendar { get; set; }
        public DbSet<Unavailability> Unavailabilities { get; set; }
        public DbSet<UnavailabilityDate> UnavailabilityDate { get; set; }
        public DbSet<AvailabilityDateRange> AvailabilityDateRange { get; set; }
        public DbSet<AvailabilityDate> AvailabilityDate { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        #endregion
    }
}
