using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class ClassSubject : BaseEntity
    {
        public string Title { get; set; }
        public SubjectAssignment SubjectAssignment { get; set; }
        public Guid SubjectAssignmentId { get; set; }
        public CourseClass CourseClass { get; set; }
        public Guid CourseClassId { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
