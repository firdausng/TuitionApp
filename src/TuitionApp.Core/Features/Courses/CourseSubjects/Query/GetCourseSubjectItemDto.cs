using System;

namespace TuitionApp.Core.Features.Courses.CourseSubjects
{
    public class GetCourseSubjectItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CourseId { get; set; }
        public Guid SubjectAssignmentId { get; set; }
    }
}
