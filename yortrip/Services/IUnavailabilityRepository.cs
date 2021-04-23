using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using yortrip.Entities;

namespace yortrip.Services
{
    public interface IUnavailabilityRepository
    {
        Unavailability GetUnavailability(Guid unavailabilityId);

        Task<List<UnavailabilityDate>> GetUnavailability(Guid calendarId, string duration, string userIds);

        Task<List<AvailabilityDateRange>> GetAvailabilityDateRanges(Guid calendarId, string duration, string userIds);

        Task<List<AvailabilityDate>> GetAvailabilityDates(Guid calendarId, string duration, string userIds);

        void AddUnavailabilities(List<DateTime> unavailabilities, Guid userId, Guid calendarId);

        void UpdateUnavailabilitie(Unavailability unavailability);

        void DeleteUnavailabilitie(Unavailability unavailability);

        bool SaveChanges();
    }
}
