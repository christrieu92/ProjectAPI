using System;

namespace yortrip.Dto
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string FireBaseUID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? Enddate { get; set; }
    }
}
