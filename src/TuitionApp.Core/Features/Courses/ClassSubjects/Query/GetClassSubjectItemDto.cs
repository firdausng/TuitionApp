using System;

namespace TuitionApp.Core.Features.Courses.ClassSubjects
{
    public class GetClassSubjectItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CourseId { get; set; }
        public Guid SubjectAssignmentId { get; set; }
    }
}
