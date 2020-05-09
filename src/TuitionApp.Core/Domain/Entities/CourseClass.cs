using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class CourseClass : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int SeatTaken { get; set; }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
