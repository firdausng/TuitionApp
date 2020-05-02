using System;

namespace TuitionApp.Core.Features.DailySchedules.Timeslots
{
    public class GetTimeslotItemDto
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; }
        public Guid DailyScheduleId { get; set; }
    }
}
