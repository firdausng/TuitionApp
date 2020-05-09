using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid CourseClassId { get; set; }
        public CourseClass CourseClass { get; set; }
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
