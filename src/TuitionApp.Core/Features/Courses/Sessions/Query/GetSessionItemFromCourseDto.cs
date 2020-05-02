using System;

namespace TuitionApp.Core.Features.Courses
{
    public class GetSessionItemFromCourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

    }
}
