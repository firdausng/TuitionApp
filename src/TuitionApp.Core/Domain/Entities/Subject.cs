using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    }
}
