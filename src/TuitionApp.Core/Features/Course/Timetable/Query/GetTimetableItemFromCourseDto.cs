using System;

namespace TuitionApp.Core.Features.Course
{
    public class GetTimetableItemFromCourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

    }
}
