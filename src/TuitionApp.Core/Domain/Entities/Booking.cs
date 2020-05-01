using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class Booking
    {
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public Guid TimeslotId { get; set; }
        public Timeslot Timeslot { get; set; }
    }
}
