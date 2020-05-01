using System;

namespace TuitionApp.Core.Features.DailySchedule.Timeslot.Query
{
    public class GetTimeslotItemDto
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; }
        public Guid DailyScheduleId { get; set; }
        public Guid SessionId { get; set; }
    }
}
