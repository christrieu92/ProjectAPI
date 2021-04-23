using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using yortrip.Dto;
using yortrip.Entities;
using yortrip.Services;

namespace yortrip.Controllers
{
    [Route("api/invite")]
    [ApiController]

    public class InviteController : ControllerBase
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IUserRepositroy _userRepository;
        private readonly IUnavailabilityRepository _unavailabilityRepository;
        private readonly IMapper _mapper;

        public InviteController(IInviteRepository inviteRepository, IUserRepositroy userRepositroy, IUnavailabilityRepository unavailabilityRepository, IMapper mapper)
        {
            _inviteRepository = inviteRepository ??
                throw new ArgumentNullException(nameof(inviteRepository));
            _mapper = mapper;

            _userRepository = userRepositroy ??
                throw new ArgumentNullException(nameof(userRepositroy));
            _mapper = mapper;

            _unavailabilityRepository = unavailabilityRepository ??
                throw new ArgumentException(nameof(unavailabilityRepository));
            _mapper = mapper;
        }

        /// <summary>
        /// Get a calendar based on the calendarId
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>Single Calendar</returns>
        /// Get api/invite/calendar/{calendarId}
        [HttpGet("calendar/{calendarId}", Name = "GetInviteCalendarByCalendarId")]
        public ActionResult<CalendarDto> GetInviteCalendar(Guid calendarId)
        {
            CalendarModel inviteRepo = _inviteRepository.GetCalendar(calendarId);

            if (inviteRepo == null)
            {
                return NotFound(_mapper.Map<CalendarDto>(inviteRepo));
            }

            return Ok(inviteRepo);
        }

        /// <summary>
        /// Get a specific user from the list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Single user</returns>
        /// Get api/invite/user/{userId}
        [HttpGet("user/{userId}", Name = "GetInviteeByUserId")]
        public ActionResult<UserDto> GetInviteeByEmail(Guid userId)
        {
            User userRepo = _inviteRepository.GetUser(userId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            return Ok(userRepo);
        }

        /// <summary>
        /// Adds a invitee calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="inviteeCalendarDto"></param>
        /// <returns>Add Invite Calendar</returns>
        /// POST api/invite
        [HttpPost("{calendarId}")]
        public ActionResult<UserDto> AddInviteCalendar(Guid calendarId, InviteeCalendarDto inviteeCalendarDto)
        {

            if (inviteeCalendarDto == null)
            {
                return NotFound(_mapper.Map<InviteeCalendarDto>(inviteeCalendarDto));
            }

            InviteeCalendar inviteeCalendarMap = _mapper.Map<InviteeCalendar>(inviteeCalendarDto);

            _inviteRepository.AddInviteCalendar(calendarId, inviteeCalendarMap);
            _inviteRepository.SaveChanges();

            // Map back to the Dto to return to the API
            InviteeCalendarDto userMapDto = _mapper.Map<InviteeCalendarDto>(inviteeCalendarMap);

            return CreatedAtRoute("GetInviteeByUserId",
                new
                {
                    userId = userMapDto.UserId
                }, userMapDto);
        }

        /// <summary>
        /// Get list of user associated to the calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>Single user</returns>
        /// Get api/user/invite/calendar/{calendarId}
        [HttpGet("user/calendar/{calendarId}")]
        public ActionResult<List<UserDto>> GetInvitedUsersByCalendar(Guid calendarId)
        {
            List<User> userRepo = _userRepository.GetUsersByCalendar(calendarId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            return Ok(userRepo);
        }

        /// <summary>
        /// Get Invite Unavailability Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get Invite Unavailability Calendar</returns>
        /// GET api/invite
        [HttpGet("calendar/{calendarId}/unavailability/{duration}/{userIds}")]
        public async Task<ActionResult<List<UnavailabilityDate>>> GetInviteCalendarUnavailability(Guid calendarId, string duration, string userIds)
        {
            List<UnavailabilityDate> unavailabilityDates = await _unavailabilityRepository.GetUnavailability(calendarId, duration, userIds);

            if (unavailabilityDates == null)
            {
                return NotFound(_mapper.Map<List<UnavailabilityDate>>(unavailabilityDates));
            }

            return Ok(unavailabilityDates);
        }

        /// <summary>
        /// Get Invite Unavailability Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get Invite Unavailability Calendar</returns>
        /// GET api/invite
        [HttpGet("calendar/{calendarId}/availabilityrange/{duration}/{userIds}")]
        public async Task<ActionResult<List<UnavailabilityDate>>> GetInviteCalendarAvailabilityRange(Guid calendarId, string duration, string userIds)
        {
            List<AvailabilityDateRange> availabilityDateRanges = await _unavailabilityRepository.GetAvailabilityDateRanges(calendarId, duration, userIds);

            if (availabilityDateRanges == null)
            {
                return NotFound(_mapper.Map<List<AvailabilityDateRange>>(availabilityDateRanges));
            }

            return Ok(availabilityDateRanges);
        }

        /// <summary>
        /// Get Invite Availability Calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="duration"></param>
        /// <param name="userIds"></param>
        /// <returns>Get Availabilities Calendar</returns>
        /// GET api/invite
        [HttpGet("calendar/{calendarId}/availability/{duration}/{userIds}")]
        public async Task<ActionResult<List<AvailabilityDate>>> GetInviteCalendarAvailability(Guid calendarId, string duration, string userIds)
        {
            List<AvailabilityDate> availabilityDates = await _unavailabilityRepository.GetAvailabilityDates(calendarId, duration, userIds);

            if (availabilityDates == null)
            {
                return NotFound(_mapper.Map<List<AvailabilityDate>>(availabilityDates));
            }

            return Ok(availabilityDates);
        }
    }
}
