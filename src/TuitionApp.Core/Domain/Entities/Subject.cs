using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<SubjectAssignment> SubjectAssignments { get; set; } = new List<SubjectAssignment>();
    }
}
