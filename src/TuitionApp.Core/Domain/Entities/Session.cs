using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Session : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<InstructorSession> InstructorSessions { get; set; } = new List<InstructorSession>();
    }
}
