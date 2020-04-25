using System;

namespace TuitionApp.Core.Features.Course
{
    public class GetCourseItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
    }
}
