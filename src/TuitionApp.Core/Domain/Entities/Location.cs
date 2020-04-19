using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
        public ICollection<LocationTeacher> LocationTeachers { get; set; } = new List<LocationTeacher>();
    }
}
