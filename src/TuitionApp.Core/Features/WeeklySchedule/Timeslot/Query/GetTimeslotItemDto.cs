using System;

namespace TuitionApp.Core.Features.WeeklySchedule.Timeslot
{
    public class GetTimeslotItemDto
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; }
        public Guid WeeklyScheduleId { get; set; }
        public Guid SessionId { get; set; }
    }
}
