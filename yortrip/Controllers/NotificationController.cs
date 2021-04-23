using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using yortrip.Dto;
using yortrip.Entities;
using yortrip.Services;


namespace yortrip.Controllers
{
    [Route("api/notification")]
    [ApiController]

    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepositroy _userRepositroy;
        private readonly IUnavailabilityRepository _unavailabilityRepository;
        private readonly IMapper _mapper;

        public NotificationController(INotificationRepository notificationRepository, IUserRepositroy userRepositroy, IUnavailabilityRepository unavailabilityRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository ??
                throw new ArgumentNullException(nameof(notificationRepository));
            _mapper = mapper;

            _userRepositroy = userRepositroy ??
                throw new ArgumentNullException(nameof(userRepositroy));
            _mapper = mapper;

            _unavailabilityRepository = unavailabilityRepository ??
                throw new ArgumentException(nameof(unavailabilityRepository));
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all the notifications for a calendar
        /// <param name="userId"></param>
        /// </summary>
        /// <returns>List of Notifications</returns>
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<NotificationDto>> GetNotifications(Guid userId)
        {
            IEnumerable<Notification> notificationRepo = _notificationRepository.GetNotifications(userId);

            IEnumerable<NotificationDto> notificationDto = _mapper.Map<IEnumerable<NotificationDto>>(notificationRepo);

            return Ok(notificationDto);
        }

        /// <summary>
        /// Adds notification
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="userId"></param>
        /// <returns>Add Notification</returns>
        /// POST api/calendar/{userid}
        [HttpPost("{userId}/{calendarId}")]
        public IActionResult AddNotification(Guid userId, Guid calendarId)
        {
            _notificationRepository.AddNotification(calendarId, userId);
            _notificationRepository.SaveChanges();

            return StatusCode(200);
        }

        /// <summary>
        /// Update notification
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notificationKey"></param>
        /// <returns>Update Notification</returns>
        /// Put api/notification/{notificationId}
        [HttpPut("{userId}/{notificationKey}")]
        public IActionResult UpdateNotification(Guid userId, string notificationKey)
        {
            List<Notification> notifications = _notificationRepository.GetNotifications(userId).ToList();

            _notificationRepository.UpdateNotification(userId, notifications, notificationKey);
            //_notificationRepository.SaveChanges();

            return StatusCode(200);
        }

        /// <summary>
        /// Get notification key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Update Notification</returns>
        /// Put api/notification/{notificationId}
        [HttpGet("notificationkey/{userId}")]
        public IActionResult GetNotificationKey(Guid userId)
        {
            string notificationKey = _notificationRepository.GetNotificationKey(userId);

            //_notificationRepository.SaveChanges();

            return Ok(notificationKey);
        }
    }
}
