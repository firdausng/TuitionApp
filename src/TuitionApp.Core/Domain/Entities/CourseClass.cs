using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class CourseClass : BaseEntity
    {
        public string Name { get; set; }

        private int capacity;
        public int Capacity
        {
            // because when set this, normal people will do without assumption number start at 0
            get { return capacity - 1; }
            set { capacity = value; }
        }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
