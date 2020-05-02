using System;

namespace TuitionApp.Core.Features.CalendarSettings
{
    public class GetCalendarSettingItemDto
    {
        public Guid Id { get; set; }
        public DayOfWeek FirstDayOfWeek { get; set; }
        public TimeSpan DefaultOpeningTime { get; set; }
        public TimeSpan DefaultClosingTime { get; set; }
        public bool AllowedTimeslotOverlap { get; set; }
    }
}
