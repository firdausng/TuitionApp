using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class InstructorSession
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }
}
