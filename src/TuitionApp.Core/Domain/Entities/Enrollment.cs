using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public int Grade { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid StudentId { get; set; }
        public Classroom Classroom { get; set; }
    }
}
