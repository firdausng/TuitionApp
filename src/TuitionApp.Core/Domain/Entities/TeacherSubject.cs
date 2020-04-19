using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class TeacherSubject
    {
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
