using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Instructor : Person
    {
        public DateTime HireDate { get; set; }
        public ICollection<LocationInstructor> LocationInstructors { get; set; } = new List<LocationInstructor>();
        public ICollection<InstructorSession> InstructorSessions { get; set; } = new List<InstructorSession>();
        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();
    }
}
