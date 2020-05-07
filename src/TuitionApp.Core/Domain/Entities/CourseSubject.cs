using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class CourseSubject : BaseEntity
    {
        public string Title { get; set; }
        public SubjectAssignment SubjectAssignment { get; set; }
        public Guid SubjectAssignmentId { get; set; }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }

    }
}
