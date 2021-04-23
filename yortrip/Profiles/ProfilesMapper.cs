using AutoMapper;
using yortrip.Dto;
using yortrip.Entities;

namespace yortrip.Profiles
{
    public class ProfilesMapper : Profile
    {
        public ProfilesMapper()
        {
            // Soruce -> Target
            // Calendar Mapping
            CreateMap<CalendarModel, CalendarDto>();

            CreateMap<CalendarCreateDto, CalendarUnavailability>();

            CreateMap<CalendarCreateDto, CalendarModel>();

            CreateMap<CalendarModel, CalendarCreateDto>();

            CreateMap<CalenadrEntity, CalendarDto>();

            CreateMap<CalendarUnavailability, CalendarDto>();

            // User Mapping
            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>();

            // Invitee Mapping
            CreateMap<InviteeCalendar, InviteeCalendarDto>();

            CreateMap<InviteeCalendarDto, InviteeCalendar>();

            CreateMap<NotificationDto, Notification>();

            CreateMap<Notification, NotificationDto>();
        }
    }
}
