using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using yortrip.DbContexts;
using yortrip.Entities;

namespace yortrip.Services
{
    public class UnavailabilityRepository : IUnavailabilityRepository, IDisposable
    {
        private readonly CalenderContext _context;

        public UnavailabilityRepository(CalenderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Unavailability GetUnavailability(Guid unavailabilityId)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public async Task<List<UnavailabilityDate>> GetUnavailability(Guid calendarId, string duration, string userIds)
        {
            SqlParameter calendarParam = new SqlParameter("@CalendarId", calendarId);

            SqlParameter durationParam = new SqlParameter("@Duration", duration);

            SqlParameter userIdsParam = new SqlParameter("@UserList", userIds);

            List<UnavailabilityDate> dates = await _context.UnavailabilityDate.FromSql("pUnavailabilities @CalendarId, @Duration, @UserList", calendarParam, durationParam, userIdsParam).ToListAsync();

            return dates;
        }

        [Obsolete]
        public async Task<List<AvailabilityDateRange>> GetAvailabilityDateRanges(Guid calendarId, string duration, string userIds)
        {
            SqlParameter calendarParam = new SqlParameter("@CalendarId", calendarId);

            SqlParameter durationParam = new SqlParameter("@Duration", duration);

            SqlParameter userIdsParam = new SqlParameter("@UserList", userIds);

            List<AvailabilityDateRange> dateRanges = await _context.AvailabilityDateRange.FromSql("pUnavailabilitiesRanges @CalendarId, @Duration, @UserList", calendarParam, durationParam, userIdsParam).ToListAsync();

            return dateRanges;

        }

        [Obsolete]
        public async Task<List<AvailabilityDate>> GetAvailabilityDates(Guid calendarId, string duration, string userIds)
        {
            SqlParameter calendarParam = new SqlParameter("@CalendarId", calendarId);

            SqlParameter durationParam = new SqlParameter("@Duration", duration);

            SqlParameter userIdsParam = new SqlParameter("@UserList", userIds);

            List<AvailabilityDate> dateRanges = await _context.AvailabilityDate.FromSql("pAvailabilities @CalendarId, @Duration, @UserList", calendarParam, durationParam, userIdsParam).ToListAsync();

            return dateRanges;

        }

        public void AddUnavailabilities(List<DateTime> unavailabilities, Guid userId, Guid calendarId)
        {
            if (unavailabilities == null)
            {
                throw new ArgumentNullException(nameof(unavailabilities));
            }


            foreach (var unavailableDate in unavailabilities)
            {
                Unavailability unavailability = new Unavailability
                {
                    UserId = userId,
                    CalendarId = calendarId,
                    UnavailabilityDate = unavailableDate
                };

                _context.Unavailabilities.Add(unavailability);
            }
        }

        public void UpdateUnavailabilitie(Unavailability unavailability)
        {
            throw new NotImplementedException();
        }

        public void DeleteUnavailabilitie(Unavailability unavailability)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

    }
}
