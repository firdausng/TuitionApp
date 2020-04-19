using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class LocationTeacher
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
    }
}
