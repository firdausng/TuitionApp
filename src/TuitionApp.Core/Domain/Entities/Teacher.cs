using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
        public ICollection<LocationTeacher> LocationTeachers { get; set; } = new List<LocationTeacher>();
    }
}
