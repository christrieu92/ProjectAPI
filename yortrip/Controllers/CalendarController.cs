using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using yortrip.Dto;
using yortrip.Entities;
using yortrip.Services;


namespace yortrip.Controllers
{
    // api/calendars
    [EnableCors("AllowMyOrigin")]
    [Authorize]
    [Route("api/calendars")]
    [ApiController]

    public class CalendarController : ControllerBase
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserRepositroy _userRepositroy;
        private readonly IUnavailabilityRepository _unavailabilityRepository;
        private readonly IMapper _mapper;

        public CalendarController(ICalendarRepository calendarRepository, IUserRepositroy userRepositroy, IUnavailabilityRepository unavailabilityRepository, IMapper mapper)
        {
            _calendarRepository = calendarRepository ??
                throw new ArgumentNullException(nameof(calendarRepository));
            _mapper = mapper;

            _userRepositroy = userRepositroy ??
                throw new ArgumentNullException(nameof(userRepositroy));
            _mapper = mapper;

            _unavailabilityRepository = unavailabilityRepository ??
                throw new ArgumentException(nameof(unavailabilityRepository));
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all the calendars in the system
        /// </summary>
        /// <returns>List of Calendars</returns>
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<CalendarDto>> GetCalendars(Guid userId)
        {
            IEnumerable<CalenadrEntity> calendarsRepo = _calendarRepository.GetCalendars(userId);

            IEnumerable<CalendarDto> calendarDto = _mapper.Map<IEnumerable<CalendarDto>>(calendarsRepo);

            return Ok(calendarDto);
        }

        /// <summary>
        /// Get a specific calendar from the lists
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>Single calendar</returns>
        /// Get api/calendar/{calendarid}
        [HttpGet("calendar/{calendarid}", Name = "GetCalendar")]
        public ActionResult<CalendarDto> GetCalendar(Guid calendarId)
        {
            CalendarModel calendarRepo = _calendarRepository.GetCalendar(calendarId);

            if (calendarRepo == null)
            {
                return NotFound(_mapper.Map<CalendarDto>(calendarRepo));
            }

            return Ok(calendarRepo);
        }

        /// <summary>
        /// Get unavailable dates for a calendarId
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get available dates for a calendarId</returns>
        /// Get api/calendar/{calendarid}/unavailability/{duration}
        [HttpGet("calendar/{calendarid}/unavailability/{duration}/{userIds}", Name = "GetCalendarUnavailability")]
        public async Task<ActionResult<List<UnavailabilityDate>>> GetCalendarUnavailability(Guid calendarId, string duration, string userIds)
        {
            List<UnavailabilityDate> unavailabilityDates = await _unavailabilityRepository.GetUnavailability(calendarId, duration, userIds);

            if (unavailabilityDates == null)
            {
                return NotFound(_mapper.Map<List<UnavailabilityDate>>(unavailabilityDates));
            }

            return Ok(unavailabilityDates);
        }

        /// <summary>
        /// Get available date ranges for a calendarId
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get available dates for a calendarId</returns>
        /// Get api/calendar/{calendarid}/availability/{duration}
        [HttpGet("calendar/{calendarid}/availabilityrange/{duration}/{userIds}", Name = "GetCalendarAvailabilityRange")]
        public async Task<ActionResult<List<AvailabilityDateRange>>> GetCalendarAvailabilityRange(Guid calendarId, string duration, string userIds)
        {
            List<AvailabilityDateRange> availabilityDateRanges = await _unavailabilityRepository.GetAvailabilityDateRanges(calendarId, duration, userIds);

            if (availabilityDateRanges == null)
            {
                return NotFound(_mapper.Map<List<AvailabilityDateRange>>(availabilityDateRanges));
            }

            return Ok(availabilityDateRanges);
        }

        /// <summary>
        /// Get available date ranges for a calendarId
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get available dates for a calendarId</returns>
        /// Get api/calendar/{calendarid}/availability/{duration}
        [HttpGet("calendar/{calendarid}/availability/{duration}/{userIds}", Name = "GetCalendarAvailability")]
        public async Task<ActionResult<List<AvailabilityDate>>> GetCalendarAvailability(Guid calendarId, string duration, string userIds)
        {
            List<AvailabilityDate> availabilityDates = await _unavailabilityRepository.GetAvailabilityDates(calendarId, duration, userIds);

            if (availabilityDates == null)
            {
                return NotFound(_mapper.Map<List<AvailabilityDateRange>>(availabilityDates));
            }

            return Ok(availabilityDates);
        }

        /// <summary>
        /// Adds a calendar from the dto to the system
        /// </summary>
        /// <param name="calendarCreateDto"></param>
        /// <param name="userId"></param>
        /// <returns>Add calendar</returns>
        /// POST api/calendar/{userid}
        [HttpPost("calendar/{userId}")]
        public ActionResult<CalendarDto> AddCalendar(Guid userId, CalendarCreateDto calendarCreateDto)
        {
            CalendarUnavailability calendarMap = _mapper.Map<CalendarUnavailability>(calendarCreateDto);
            _calendarRepository.AddCalendar(userId, calendarMap);
            _calendarRepository.SaveChanges();

            // Map back to the Dto to return to the API
            CalendarDto calendarDto = _mapper.Map<CalendarDto>(calendarMap);

            return CreatedAtRoute(nameof(GetCalendar),
                new
                {
                    calendarid = calendarDto.CalendarId
                }, calendarDto);
        }

        /// <summary>
        /// Updates the calendar based on the calendarId
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="calendarUpdateDto"></param>
        /// <returns>No content</returns>
        /// Put api/calendar/{calendarid}
        [HttpPut("calendar/{calendarid}")]
        public ActionResult UpdateCalendar(Guid calendarId, CalendarCreateDto calendarUpdateDto)
        {
            var calendarRepo = _calendarRepository.GetCalendar(calendarId);

            if (calendarRepo == null)
            {
                return NotFound(_mapper.Map<CalendarDto>(calendarRepo));
            }

            _mapper.Map(calendarUpdateDto, calendarRepo);

            _calendarRepository.UpdateCalendar(calendarRepo);

            _calendarRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Updates a partial (specific) part of the calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="patchCalendar"></param>
        /// <returns>No content</returns>
        /// Patch api/calendar/{calendarid}
        [HttpPatch("calendar/{calendarid}")]
        public ActionResult PartialCalendarUpdate(Guid calendarId, JsonPatchDocument<CalendarCreateDto> patchCalendar)
        {
            var calendarRepo = _calendarRepository.GetCalendar(calendarId);

            if (calendarRepo == null)
            {
                return NotFound(_mapper.Map<CalendarDto>(calendarRepo));
            }

            // Maps the Domain object from repository into an Dto controller object
            var calendarToPatch = _mapper.Map<CalendarCreateDto>(calendarRepo);

            // Apply patch to the dto object
            patchCalendar.ApplyTo(calendarToPatch, ModelState);

            // Validate if any problems applying patch
            if (!TryValidateModel(calendarToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(calendarToPatch, calendarRepo);

            _calendarRepository.UpdateCalendar(calendarRepo);

            _calendarRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete a calendar entry
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>No content</returns>
        /// Delete api/calendar/{calendarid}
        [HttpDelete("calendar/{calendarid}")]
        public ActionResult DeleteCalendar(Guid calendarId)
        {
            var calendarRepo = _calendarRepository.GetCalendar(calendarId);

            if (calendarRepo == null)
            {
                return NotFound(_mapper.Map<CalendarDto>(calendarRepo));
            }

            _calendarRepository.DeleteCalendar(calendarRepo);

            _calendarRepository.SaveChanges();

            return NoContent();
        }
    }
}
