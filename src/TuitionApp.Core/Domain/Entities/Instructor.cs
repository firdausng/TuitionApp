using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Instructor : Person
    {
        public DateTime HireDate { get; set; }
        public ICollection<LocationInstructor> LocationInstructors { get; set; } = new List<LocationInstructor>();
        public ICollection<SubjectAssignment> SubjectAssignments { get; set; } = new List<SubjectAssignment>();
    }
}
