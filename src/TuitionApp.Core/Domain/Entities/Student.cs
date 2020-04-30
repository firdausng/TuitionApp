using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Student: Person
    {
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
