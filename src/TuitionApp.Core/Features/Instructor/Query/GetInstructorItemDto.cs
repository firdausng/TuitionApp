using System;

namespace TuitionApp.Core.Features.Instructor
{
    public class GetInstructorItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
    }
}
