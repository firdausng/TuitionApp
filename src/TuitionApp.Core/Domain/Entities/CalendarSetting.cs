using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class CalendarSetting : BaseEntity
    {
        public DayOfWeek FirstDayOfWeek { get; set; }
        public TimeSpan DefaultOpeningTime { get; set; }
        public TimeSpan DefaultClosingTime { get; set; }
        public bool AllowedTimeslotOverlap { get; set; }
    }
}
