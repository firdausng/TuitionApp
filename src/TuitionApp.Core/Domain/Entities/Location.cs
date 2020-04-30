using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string Address { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
        public ICollection<LocationInstructor> LocationInstructors { get; set; } = new List<LocationInstructor>();
    }
}
