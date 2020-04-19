using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Classroom : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int CapacityLeft { get; set; }
        public Guid LocationId { get; set; }
        public Guid SubjectId { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
