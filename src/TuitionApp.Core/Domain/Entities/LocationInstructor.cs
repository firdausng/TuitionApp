using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class LocationInstructor
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
    }
}
