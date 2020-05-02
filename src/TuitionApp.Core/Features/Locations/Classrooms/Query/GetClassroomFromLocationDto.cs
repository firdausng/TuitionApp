using System;

namespace TuitionApp.Core.Features.Locations.Classrooms
{
    public class GetClassroomFromLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

    }
}
