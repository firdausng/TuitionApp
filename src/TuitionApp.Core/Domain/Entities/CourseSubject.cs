using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class CourseSubject
    {
        public Subject Subject { get; set; }
        public Guid SubjectId { get; set; }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
    }
}
