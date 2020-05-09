using System;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class GetCourseClassItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int CapacityLeft { get; set; }
        public Guid CourseId { get; set; }
    }
}
