using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class Timeslot: BaseEntity
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public bool Disabled { get; set; } 
        public Guid WeeklyScheduleId { get; set; }
        public WeeklySchedule WeeklySchedule { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }


    public static class TimeslotExtension
    {
        public static bool IsTimeslotOverlapping(this Timeslot currentTimeslot, ICollection<Timeslot> timeslots)
        {
            return timeslots.IsTimeslotOverlapping(currentTimeslot.StartTime, currentTimeslot.Duration);
        }

        public static bool IsTimeslotOverlapping(this ICollection<Timeslot> timeslots, TimeSpan startTime, TimeSpan duration)
        {
            foreach (var timeslot in timeslots)
            {
                if (timeslot.StartTime > startTime && timeslot.StartTime < startTime.Add(duration))
                {
                    return true;
                }
                else if (startTime > timeslot.StartTime && startTime < timeslot.StartTime.Add(timeslot.Duration))
                {
                    return true;
                }
            }
            return false;
        }

        public static ICollection<Timeslot> GetOverlapTimeslot(this ICollection<Timeslot> timeslots, TimeSpan startTime, TimeSpan duration)
        {
            List<Timeslot> overlapTimeslots = new List<Timeslot>();

            foreach (var timeslot in timeslots)
            {
                if (timeslot.StartTime > startTime && timeslot.StartTime < startTime.Add(duration))
                {
                    overlapTimeslots.Add(timeslot);
                }
                else if (startTime > timeslot.StartTime && startTime < timeslot.StartTime.Add(timeslot.Duration))
                {
                    overlapTimeslots.Add(timeslot);
                }
            }

            return overlapTimeslots;
        }
    }
}
