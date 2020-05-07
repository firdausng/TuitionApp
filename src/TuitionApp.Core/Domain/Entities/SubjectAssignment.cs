using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class SubjectAssignment : BaseEntity
    {
        public Subject Subject { get; set; }
        public Guid SubjectId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid InstructorId { get; set; }
        public ICollection<ClassSubject> CourseSubjects { get; set; } = new List<ClassSubject>();
    }
}
