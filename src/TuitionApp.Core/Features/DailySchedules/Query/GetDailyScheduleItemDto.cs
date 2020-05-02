using System;

namespace TuitionApp.Core.Features.DailySchedules
{
    public class GetDailyScheduleItemDto
    {
        public Guid Id { get; set; }
        public DateTime DateSchedule { get; set; }
        public int WeekNumber { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool Disabled { get; set; }
        public Guid ClassroomId { get; set; }
    }
}
