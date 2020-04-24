using System;

namespace TuitionApp.Core.Features.Location
{
    public class GetClassroomFromLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

    }
}
