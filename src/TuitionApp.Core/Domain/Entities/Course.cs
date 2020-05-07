using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public ICollection<CourseClass> CourseClasses { get; set; } = new List<CourseClass>();
        public ICollection<ClassSubject> CourseSubjects { get; set; } = new List<ClassSubject>();
    }
}
