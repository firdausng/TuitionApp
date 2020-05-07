using System;
using System.Collections;
using System.Collections.Generic;

namespace TuitionApp.Core.Features.Subjects
{
    public class GetSubjectItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<Guid> SubjectAssignmentList { get; set; }
    }
}
