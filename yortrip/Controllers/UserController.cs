using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using yortrip.Dto;
using yortrip.Entities;
using yortrip.Services;

namespace yortrip.Controllers
{
    // api/users
    [EnableCors("AllowMyOrigin")]
    [Authorize]
    [Route("api/user")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepositroy _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepositroy userRepository, IMapper mapper)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper;
        }

        /// <summary>
        /// Get a specific user from the list
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Single user</returns>
        /// Get api/user/{userId}
        [HttpGet("email/{email}", Name = "GetUserByEmail")]
        public ActionResult<UserDto> GetUserByEmail(string email)
        {
            User userRepo = _userRepository.GetUser(email);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            return Ok(userRepo);
        }

        /// <summary>
        /// Get list of user associated to the calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>Single user</returns>
        /// Get api/user/{userId}
        [HttpGet("calendar/{calendarId}")]
        public ActionResult<List<UserDto>> GetUsersByCalendar(Guid calendarId)
        {
            List<User> userRepo = _userRepository.GetUsersByCalendar(calendarId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            return Ok(userRepo);
        }

        /// <summary>
        /// Adds a user from the dto to the system
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Add user</returns>
        /// POST api/user
        [HttpPost]
        public ActionResult<UserDto> AddUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            _userRepository.AddUser(user);
            _userRepository.SaveChanges();

            // Map back to the Dto to return to the API
            UserDto userMapDto = _mapper.Map<UserDto>(user);

            return CreatedAtRoute("GetUserByEmail",
                new
                {
                    email = userMapDto.Email
                }, userMapDto);
        }

        /// <summary>
        /// Updates the user based on the userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDto"></param>
        /// <returns>No content</returns>
        /// Put api/calendar/{userId}
        [HttpPut("{userId}")]
        public ActionResult UpdateUser(Guid userId, UserDto userDto)
        {
            var userRepo = _userRepository.GetUser(userId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            _mapper.Map(userDto, userRepo);

            _userRepository.UpdateUser(userRepo);

            _userRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Updates a partial (specific) part of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="patchUser"></param>
        /// <returns>No content</returns>
        /// Patch api/user/{userId}
        [HttpPatch("{userId}")]
        public ActionResult PartialUserUpdate(Guid userId, JsonPatchDocument<UserDto> patchUser)
        {
            var userRepo = _userRepository.GetUser(userId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            // Maps the Domain object from repository into an Dto controller object
            var userToPatch = _mapper.Map<UserDto>(userRepo);

            // Apply patch to the dto object
            patchUser.ApplyTo(userToPatch, ModelState);

            // Validate if any problems applying patch
            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userRepo);

            _userRepository.UpdateUser(userRepo);

            _userRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete a user entry
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>No content</returns>
        /// Delete api/user/{userId}
        [HttpDelete("{userId}")]
        public ActionResult DeleteUser(Guid userId)
        {
            var userRepo = _userRepository.GetUser(userId);

            if (userRepo == null)
            {
                return NotFound(_mapper.Map<UserDto>(userRepo));
            }

            _userRepository.DeleteUser(userRepo);

            _userRepository.SaveChanges();

            return NoContent();
        }
    }
}
