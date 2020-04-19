using System;

namespace TuitionApp.Core.Features.Classroom
{
    public class GetSubjectDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Guid Id { get; set; }
    }
}
