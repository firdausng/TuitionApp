using System;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class GetCourseClassItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CourseId { get; set; }
    }
}
