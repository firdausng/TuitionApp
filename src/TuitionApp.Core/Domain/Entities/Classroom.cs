using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Classroom : BaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int Capacity { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
