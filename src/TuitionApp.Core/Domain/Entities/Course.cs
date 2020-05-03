using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();
    }
}
