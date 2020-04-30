using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class WeeklySchedule : BaseEntity
    {
        public DateTime DateSchedule { get; set; }
        public int WeekNumber { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool Disabled { get; set; }
        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
