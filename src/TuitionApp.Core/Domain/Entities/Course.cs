using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
