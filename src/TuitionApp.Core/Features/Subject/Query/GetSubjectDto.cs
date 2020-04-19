using System;

namespace TuitionApp.Core.Features.Subject
{
    public class GetSubjectDto
    {
        public string Title { get; set; }
        public int Capacity { get; set; }
        public Guid Id { get; set; }
    }
}
