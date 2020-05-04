using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Title { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Instructor Instructor { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
